using System;
using System.Collections.Generic;
using System.Linq;


namespace Core
{
    public abstract class State
    {
        public abstract void Refresh();
        public abstract void OnEnter();
        public abstract void OnExit();
    }

    public class StateMachine
    {
        public State CurrentState { get; private set; }
        private readonly Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
        private List<Transition> _currentTransitions = new List<Transition>();
        private readonly List<Transition> _anyTransition = new List<Transition>();
        
        private static readonly List<Transition> EmptyTransitions = new List<Transition>(0);

        public void SetState(State state)
        {
            if (state == CurrentState) return;
           
            CurrentState?.OnExit();
            CurrentState = state;

            _transitions.TryGetValue(CurrentState.GetType(), out _currentTransitions);
            _currentTransitions ??= EmptyTransitions;
            
            CurrentState.OnEnter();
        }

        public void AddTransition(State from, State to, Func<bool> condition)
        {
            if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
            {
                transitions = new List<Transition>();
                _transitions[from.GetType()] = transitions;
            }
            
            transitions.Add(new Transition(to, condition));
        }

        public void AddAnyTransition(State state, Func<bool> condition)
        {
            _anyTransition.Add(new Transition(state, condition));
        }
        
        public void Refresh()
        {
            var transition = GetTransition();
            if (transition != null) SetState(transition.NextState);
            CurrentState?.Refresh();
        }

        private class Transition
        {
            public Func<bool> Condition { get; }
            public State NextState { get; }

            public Transition(State nextState, Func<bool> condition)
            {
                NextState = nextState;
                Condition = condition;
            }
        }

        private Transition GetTransition()
        {
            foreach (var transition in _anyTransition.Where(transition => transition.Condition()))
            {
                return transition;
            }

            return _currentTransitions.FirstOrDefault(transition => transition.Condition());
        }
    }
}