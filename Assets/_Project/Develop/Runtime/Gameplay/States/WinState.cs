using _Project.Develop.Runtime.Gameplay.Features.InputFeature;
using _Project.Develop.Runtime.Utilities.StateMachineCore;

namespace _Project.Develop.Runtime.Gameplay.States
{
    public class WinState : EndGameState, IUpdatableState
    {
        public WinState(IInputService inputService) : base(inputService)
        {
        }

        public override void Enter()
        {
            base.Enter();

            //WinPopup
            //LevelProgressionService.AddToLevelCompleted(GameplayInputArgs.LevelNumber)
            //PlayerDataProvider.Save
        }

        public void Update(float deltaTime)
        {
            
        }
    }
}
