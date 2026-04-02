using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueCard.Core
{
    /// <summary>
    /// Manages transitions between game states.
    /// States are registered once and reused (no allocation on transition).
    /// </summary>
    public class GameStateMachine
    {
        private readonly Dictionary<Type, GameState> _states = new();
        private GameState _currentState;

        public GameState CurrentState => _currentState;
        public Type CurrentStateType => _currentState?.GetType();

        public void RegisterState(GameState state)
        {
            var type = state.GetType();
            if (!_states.ContainsKey(type))
                _states[type] = state;
        }

        public void TransitionTo<T>() where T : GameState
        {
            TransitionTo(typeof(T));
        }

        public void TransitionTo(Type stateType)
        {
            if (!_states.TryGetValue(stateType, out var nextState))
            {
                Debug.LogError($"[StateMachine] State not registered: {stateType.Name}");
                return;
            }

            if (_currentState == nextState) return;

            _currentState?.OnExit();
            _currentState = nextState;
            _currentState.OnEnter();

            Debug.Log($"[StateMachine] → {stateType.Name}");
        }

        public T GetState<T>() where T : GameState
        {
            _states.TryGetValue(typeof(T), out var state);
            return state as T;
        }

        public void Update()
        {
            _currentState?.OnUpdate();
        }
    }
}
