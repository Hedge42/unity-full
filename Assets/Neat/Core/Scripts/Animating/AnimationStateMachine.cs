using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Neat.DDD
{
    public interface IAnimState
    {
        void OnEnter();
        void OnExit();
        void OnTick();
    }

    public class AnimationStateMachine : MonoBehaviour
    {
        private IAnimState _currentState;

        private AnimationState state;

        private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
        private List<Transition> _currentTransitions;

        private readonly List<Transition> EmptyTransitions = new List<Transition>(0);

        public void SetState(IAnimState state)
        {
            if (state == _currentState)
                return;

            _currentState?.OnExit();
            _currentState = state;

            _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
            if (_currentTransitions == null)
                _currentTransitions = EmptyTransitions;
        }

        public void AddTransition(IAnimState from, IAnimState to, Func<bool> predicate)
        {
            if (!_transitions.TryGetValue(_currentState.GetType(), out var transitions))
            {
                transitions = new List<Transition>();
                _transitions[from.GetType()] = transitions;
            }
            else
            {
                transitions.Add(new Transition(to, predicate));
            }
        }

        private class Transition
        {
            IAnimState to;
            Func<bool> predicate;
            public Transition(IAnimState to, Func<bool> predicate)
            {
                this.to = to;
                this.predicate = predicate;
            }
        }
    }
}