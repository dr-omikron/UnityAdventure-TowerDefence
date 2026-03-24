using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Utilities.StateMachineCore
{
    public interface IState
    {
        IReadOnlyEvent Entered { get; }
        IReadOnlyEvent Exited { get; }

        void Enter();
        void Exit();
    }
}
