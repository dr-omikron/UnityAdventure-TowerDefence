using System;
using _Project.Develop.Runtime.Configs.Gameplay.Entities;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.AI;
using _Project.Develop.Runtime.Gameplay.Features.AI.States;
using _Project.Develop.Runtime.Gameplay.Features.Station;
using _Project.Develop.Runtime.Infrastructure.DI;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Turrets
{
    public class TurretsFactory
    {
        private readonly DIContainer _container;

        private readonly EntitiesFactory _entitiesFactory;
        private readonly BrainsFactory _brainsFactory;
        private readonly StationHolderService _stationHolderService;
        private readonly TurretsHolderService _turretsHolderService;

        public TurretsFactory(DIContainer container)
        {
            _container = container;
            _entitiesFactory = container.Resolve<EntitiesFactory>();
            _brainsFactory = container.Resolve<BrainsFactory>();
            _stationHolderService = container.Resolve<StationHolderService>();
            _turretsHolderService = container.Resolve<TurretsHolderService>();
        }

        public Entity Create(TurretConfig turretConfig)
        {
            if (_stationHolderService.Station == null)
                throw new InvalidOperationException("Station is not created yet, turret orbit center is unknown");

            Vector3 orbitCenter = _stationHolderService.Station.Transform.position
                                  + Vector3.up * turretConfig.OrbitHeight;

            float startAngle = _turretsHolderService.GetNextStartAngle();

            Entity turret = _entitiesFactory.CreateTurret(turretConfig, orbitCenter, startAngle);

            _brainsFactory.CreateTurretBrain(turret, new NearestDamageableTargetSelector(turret));
            _turretsHolderService.Add(turret);

            return turret;
        }
    }
}
