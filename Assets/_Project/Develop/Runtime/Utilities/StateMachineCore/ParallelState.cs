using System.Collections.Generic;

namespace _Project.Develop.Runtime.Utilities.StateMachineCore
{
    public abstract class ParallelState<TState> : State where TState : class, IState
    {
        private readonly List<TState> _states;

        protected ParallelState(params TState[] states)
        {
            _states = new List<TState>(states);
        }

        protected IReadOnlyList<TState> States => _states;

        public override void Enter()
        {
            base.Enter();

            foreach (var state in _states)
                state.Enter();
        }

        public override void Exit()
        {
            base.Exit();

            foreach (var state in _states)
                state.Exit();
        }
    }
}
