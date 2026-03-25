using _Project.Develop.Runtime.Gameplay.Features.InputFeature;
using _Project.Develop.Runtime.Utilities.StateMachineCore;

namespace _Project.Develop.Runtime.Gameplay.States
{
    public class DefeatState : EndGameState, IUpdatableState
    {
        public DefeatState(IInputService inputService) : base(inputService)
        {
        }

        public void Update(float deltaTime)
        {
            
        }
    }
}
