using _Project.Develop.Runtime.Utilities.StateMachineCore;

namespace _Project.Develop.Runtime.Gameplay.States
{
    public class PreparationState : State, IUpdatableState
    {

        public override void Enter()
        {
            base.Enter();

            //Create shop popup
        }

        public override void Exit()
        {
            base.Exit();
            
            //Cleanup shop popup
        }

        public void Update(float deltaTime)
        {
            
        }
    }
}
