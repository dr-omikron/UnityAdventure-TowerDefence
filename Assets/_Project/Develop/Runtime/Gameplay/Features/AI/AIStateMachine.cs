using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.AI
{
    public class AIStateMachine : StateMachine<IUpdatableState>
    {
        public AIStateMachine() : base(new List<IDisposable>()) { }

        public AIStateMachine(List<IDisposable> disposables) : base(disposables) { }

        protected override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);

            CurrentState?.Update(deltaTime);
        }
    }
}
