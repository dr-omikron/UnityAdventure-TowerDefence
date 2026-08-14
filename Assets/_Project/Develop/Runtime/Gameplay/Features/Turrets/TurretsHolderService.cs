using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.OrbitMovement;
using _Project.Develop.Runtime.Infrastructure.DI;

namespace _Project.Develop.Runtime.Gameplay.Features.Turrets
{
    public class TurretsHolderService : IInitializable, IDisposable
    {
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private readonly List<Entity> _turrets = new List<Entity>();

        public TurretsHolderService(EntitiesLifeContext entitiesLifeContext)
        {
            _entitiesLifeContext = entitiesLifeContext;
        }

        public IReadOnlyList<Entity> Turrets => _turrets;

        public void Initialize()
        {
            _entitiesLifeContext.Released += OnEntityReleased;
        }

        public float GetNextStartAngle()
        {
            if (_turrets.Count == 0)
                return 0;

            return _turrets[0].OrbitAngle.Value;
        }

        public void Add(Entity turret)
        {
            _turrets.Add(turret);
            RedistributeAngles();
        }

        public void Dispose()
        {
            _entitiesLifeContext.Released -= OnEntityReleased;
            _turrets.Clear();
        }

        private void OnEntityReleased(Entity entity)
        {
            if (_turrets.Remove(entity))
                RedistributeAngles();
        }

        private void RedistributeAngles()
        {
            if (_turrets.Count == 0)
                return;

            float baseAngle = _turrets[0].OrbitAngle.Value;
            float angleStep = OrbitGeometry.FULL_CIRCLE_DEGREES / _turrets.Count;

            for (int i = 0; i < _turrets.Count; i++)
                _turrets[i].OrbitAngle.Value = baseAngle + angleStep * i;
        }
    }
}
