using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Sensors
{
    public class DeathMaskTouchDetectorSystem : IInitializableSystem, IUpdateableSystem
    {
        private Buffer<Collider> _contacts;
        private ReactiveVariable<bool> _isTouchingDeathMask;
        private LayerMask _deathMask;

        public void OnInit(Entity entity)
        {
            _contacts = entity.ContactCollidersBuffer;
            _isTouchingDeathMask = entity.IsTouchingDeathMask;
            _deathMask = entity.DeathMask;
        }

        public void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < _contacts.Count; i++)
            {
                if (MatchWithDeathLayer(_contacts.Items[i]))
                {
                    _isTouchingDeathMask.Value = true;
                    return;
                }
            }

            _isTouchingDeathMask.Value = false;
        }

        private bool MatchWithDeathLayer(Collider collider) => (1 << collider.gameObject.layer & _deathMask) != 0;
    }
}
