namespace RogueCard.Core
{
    /// <summary>
    /// Abstract base class for all game states.
    /// </summary>
    public abstract class GameState
    {
        protected GameManager Game { get; private set; }

        public void Init(GameManager game)
        {
            Game = game;
        }

        /// <summary>Called when entering this state.</summary>
        public abstract void OnEnter();

        /// <summary>Called when leaving this state.</summary>
        public abstract void OnExit();

        /// <summary>Called every frame while this state is active.</summary>
        public virtual void OnUpdate() { }
    }
}
