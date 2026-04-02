using RogueCard.Map;
using RogueCard.Player;

namespace RogueCard.Cards
{
    public enum PlayerDecision
    {
        GoToA,          // Move to revealed node A
        GoToB,          // Move to unknown node B
        ReplaceAGoToC,  // Swap hand card C onto A's slot, move to C's place; A goes to hand
        ReplaceAGoToB   // Swap hand card C onto A's slot, move to B; C is lost
    }

    public class SwapResult
    {
        public MapNode Destination;
        public PlaceCardInstance CardAddedToHand;   // null if none gained
        public PlaceCardInstance CardLost;           // null if none lost
        public bool SwapOccurred;
    }

    /// <summary>
    /// The core mechanic resolver. Handles all four decision types atomically.
    /// This is the single point of mutation for both the tree and hand.
    /// </summary>
    public class CardSwapResolver
    {
        private readonly MapNavigator _navigator;
        private readonly PlayerState _player;

        public CardSwapResolver(MapNavigator navigator, PlayerState player)
        {
            _navigator = navigator;
            _player = player;
        }

        /// <summary>
        /// Validates whether a swap from hand onto the given node is legal.
        /// </summary>
        public bool CanSwap(PlaceCardInstance handCard, MapNode target)
        {
            if (handCard == null || target == null) return false;
            if (!_player.Hand.Contains(handCard)) return false;
            var (childA, childB) = _navigator.GetChoices();
            return target == childA || target == childB;
        }

        // ─── Decision A: Go straight to revealed node ───────────────────────

        public SwapResult ResolveGoToA(MapNode nodeA)
        {
            return new SwapResult
            {
                Destination = nodeA,
                SwapOccurred = false
            };
        }

        // ─── Decision B: Go to unknown node ─────────────────────────────────

        public SwapResult ResolveGoToB(MapNode nodeB)
        {
            return new SwapResult
            {
                Destination = nodeB,
                SwapOccurred = false
            };
        }

        // ─── Decision C: Replace A with hand card, go to C (1-for-1 swap) ───

        /// <summary>
        /// Hand card C replaces node A. Original A's card returns to hand.
        /// Player moves to where C now sits (which was A's position).
        /// </summary>
        public SwapResult ResolveReplaceAGoToC(MapNode nodeA, PlaceCardInstance handCard)
        {
            // Remove hand card
            _player.TryRemoveCardFromHand(handCard);

            // Place hand card onto node A; get the displaced card back
            var displaced = _navigator.SwapNodeCard(nodeA, handCard);

            // Displaced card (original A) goes to hand
            _player.TryAddCardToHand(displaced);

            // The destination is nodeA — but it now contains handCard
            nodeA.Card?.Reveal(); // Reveal since player chose to go here

            return new SwapResult
            {
                Destination = nodeA,
                CardAddedToHand = displaced,
                CardLost = null,
                SwapOccurred = true
            };
        }

        // ─── Decision D: Replace A with hand card, go to B (C is sacrificed) ─

        /// <summary>
        /// Hand card C replaces node A (stays on tree, not visited).
        /// Player moves to B instead. C is permanently lost.
        /// </summary>
        public SwapResult ResolveReplaceAGoToB(MapNode nodeA, PlaceCardInstance handCard, MapNode nodeB)
        {
            // Remove hand card
            _player.TryRemoveCardFromHand(handCard);

            // Place hand card onto nodeA; displaced card is discarded
            var displaced = _navigator.SwapNodeCard(nodeA, handCard);
            // displaced is intentionally dropped — it becomes a "ghost" node
            // (could be used for future gameplay flavor, e.g. reappears later)

            return new SwapResult
            {
                Destination = nodeB,
                CardAddedToHand = null,
                CardLost = handCard,
                SwapOccurred = true
            };
        }
    }
}
