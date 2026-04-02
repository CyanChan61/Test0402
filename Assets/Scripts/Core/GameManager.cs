using UnityEngine;
using RogueCard.Cards;
using RogueCard.Map;
using RogueCard.Player;
using RogueCard.Data;
using RogueCard.Events;
using RogueCard.Core.States;
using System.Linq;

namespace RogueCard.Core
{
    /// <summary>
    /// Central orchestrator. Owns all runtime systems and manages the run lifecycle.
    /// Place this on a persistent GameObject in the Bootstrap scene.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Configuration")]
        [SerializeField] private RunConfig defaultRunConfig;
        [SerializeField] private PlaceCard[] allPlaceCards;

        [Header("Events")]
        [SerializeField] private GameEventSO onNodeRevealed;
        [SerializeField] private GameEventSO onPlayerMoved;
        [SerializeField] private GameEventSO onHandChanged;
        [SerializeField] private GameEventSO onSwapExecuted;
        [SerializeField] private GameEventSO onRunEnded;

        // ── Runtime Systems ──────────────────────────────────────────────────
        public GameStateMachine StateMachine { get; private set; }
        public MapTree Map { get; private set; }
        public MapNavigator Navigator { get; private set; }
        public PlayerState Player { get; private set; }
        public CardSwapResolver SwapResolver { get; private set; }
        public NodeRevealController RevealController { get; private set; }
        public RunConfig ActiveConfig { get; private set; }

        // ── UI (assigned via Inspector or found at runtime) ──────────────────
        public UI.UIManager UIManager { get; private set; }

        // ────────────────────────────────────────────────────────────────────

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            UIManager = FindObjectOfType<UI.UIManager>();
            InitStateMachine();
        }

        private void Start()
        {
            StateMachine.TransitionTo<StateBootstrap>();
        }

        private void Update()
        {
            StateMachine.Update();
        }

        // ── Run Lifecycle ────────────────────────────────────────────────────

        public void StartNewRun(RunConfig config = null, int seed = -1)
        {
            ActiveConfig = config != null ? config : defaultRunConfig;
            if (seed < 0) seed = Random.Range(0, int.MaxValue);

            Debug.Log($"[GameManager] Starting run | seed={seed} | config={ActiveConfig.name}");

            // Build deck
            var deck = new Deck(allPlaceCards, seed);

            // Generate map
            var generator = new MapGenerator();
            Map = generator.Generate(deck, ActiveConfig, seed);

            // Init player
            Player = new PlayerState(
                ActiveConfig.startingHp,
                ActiveConfig.startingGold,
                ActiveConfig.maxHandSize
            );
            Player.MoveTo(Map.CurrentNode);

            // Fill starting hand with random cards
            var startingPool = allPlaceCards
                .Where(c => c.placeType != PlaceType.Start && c.placeType != PlaceType.Boss)
                .ToArray();
            var rng = new System.Random(seed + 1);
            for (int i = 0; i < ActiveConfig.startingHandSize && i < startingPool.Length; i++)
            {
                int idx = rng.Next(startingPool.Length);
                Player.TryAddCardToHand(startingPool[idx].CreateInstance());
            }

            // Wire up navigators and resolvers
            Navigator = new MapNavigator(Map);
            SwapResolver = new CardSwapResolver(Navigator, Player);
            RevealController = new NodeRevealController(Map, onNodeRevealed);

            StateMachine.TransitionTo<StateMapNavigation>();
        }

        public void EndRun(bool victory)
        {
            Debug.Log($"[GameManager] Run ended | victory={victory}");
            onRunEnded?.Raise(victory);

            if (victory)
                StateMachine.TransitionTo<StateGameOver>(); // GameOver handles both outcomes
            else
                StateMachine.TransitionTo<StateGameOver>();
        }

        // ── Decision Dispatch ────────────────────────────────────────────────

        public void ExecuteDecision(
            PlayerDecision decision,
            MapNode nodeA,
            PlaceCardInstance handCard = null,
            MapNode nodeB = null)
        {
            SwapResult result = decision switch
            {
                PlayerDecision.GoToA         => SwapResolver.ResolveGoToA(nodeA),
                PlayerDecision.GoToB         => SwapResolver.ResolveGoToB(nodeB),
                PlayerDecision.ReplaceAGoToC => SwapResolver.ResolveReplaceAGoToC(nodeA, handCard),
                PlayerDecision.ReplaceAGoToB => SwapResolver.ResolveReplaceAGoToB(nodeA, handCard, nodeB),
                _                            => SwapResolver.ResolveGoToA(nodeA)
            };

            // Apply movement
            Navigator.NavigateTo(result.Destination);
            Player.MoveTo(result.Destination);

            // Notify UI
            onPlayerMoved?.Raise(result.Destination);
            if (result.SwapOccurred)
                onSwapExecuted?.Raise(result);
            onHandChanged?.Raise(Player.Hand);

            // Trigger place effect
            TriggerPlaceEffect(result.Destination);
        }

        public void TriggerPlaceEffect(MapNode node)
        {
            if (node?.Card == null)
            {
                StateMachine.TransitionTo<StateMapNavigation>();
                return;
            }

            // Check win/lose conditions first
            if (!Player.IsAlive)
            {
                EndRun(victory: false);
                return;
            }

            if (Map.IsAtBoss() && node.Card.PlaceType == PlaceType.Boss)
            {
                // Boss is handled in StateCombat; victory check happens after combat
            }

            node.Card.ExecuteEffect(Player, StateMachine);
        }

        // ── State Machine Setup ──────────────────────────────────────────────

        private void InitStateMachine()
        {
            StateMachine = new GameStateMachine();

            var states = new GameState[]
            {
                new StateBootstrap(),
                new StateMainMenu(),
                new StateMapNavigation(),
                new StateRevealPhase(),
                new StateDecisionPhase(),
                new StateSwapConfirm(),
                new StateTransition(),
                new StateCombat(),
                new StateShop(),
                new StateEvent(),
                new StateRest(),
                new StateTreasure(),
                new StateGameOver()
            };

            foreach (var state in states)
            {
                state.Init(this);
                StateMachine.RegisterState(state);
            }
        }
    }
}
