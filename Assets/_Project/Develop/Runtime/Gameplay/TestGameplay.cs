using System;
using _Project.Develop.Runtime.Configs.Gameplay.Entities;
using _Project.Develop.Runtime.Configs.Gameplay.Levels;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.AI;
using _Project.Develop.Runtime.Gameplay.Features.AI.States;
using _Project.Develop.Runtime.Gameplay.Features.Enemies;
using _Project.Develop.Runtime.Gameplay.Features.InputFeature;
using _Project.Develop.Runtime.Gameplay.Features.Station;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using _Project.Develop.Runtime.Utilities.Physic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Develop.Runtime.Gameplay
{
    public class TestGameplay : MonoBehaviour
    {
        private DIContainer _container;
        private bool _isRunning;

        private Entity _station;
        private Entity _simpleEnemy;
        private Entity _shootingEnemy;
        private Entity _turret;

        private StationFactory _stationFactory;
        private EnemiesFactory _enemiesFactory;
        private EntitiesFactory _entitiesFactory;
        private BrainsFactory _brainsFactory;
        private ConfigsProviderService _configsProvider;

        private ScreenToWorldPointRaycastService _screenToWorldPointRaycastService;
        private IInputService _inputService;

        public void Initialize(DIContainer container)
        {
            _container = container;
            _inputService = _container.Resolve<IInputService>();
            _screenToWorldPointRaycastService = _container.Resolve<ScreenToWorldPointRaycastService>();

            _configsProvider = _container.Resolve<ConfigsProviderService>();
            _stationFactory = _container.Resolve<StationFactory>();
            _enemiesFactory = _container.Resolve<EnemiesFactory>();
            _entitiesFactory = _container.Resolve<EntitiesFactory>();
            _brainsFactory = _container.Resolve<BrainsFactory>();
        }

        public void Run()
        {
            _isRunning = true;
            
            LevelsListConfig levelsListConfig = _configsProvider.GetConfig<LevelsListConfig>();
            LevelConfig levelConfig = levelsListConfig.GetBy(Random.Range(1, levelsListConfig.Levels.Count + 1));

            _station = _stationFactory.Create(levelConfig);

            SimpleEnemyConfig simpleEnemyConfig = _configsProvider.GetConfig<SimpleEnemyConfig>();
            _simpleEnemy = _enemiesFactory.Create(Vector3.right * 100, simpleEnemyConfig, _station);
            /*Entity simpleEnemy2 = _enemiesFactory.Create(Vector3.forward * 100, simpleEnemyConfig, _station);

            ShootingEnemyConfig shootingEnemyConfig = _configsProvider.GetConfig<ShootingEnemyConfig>();
            _shootingEnemy = _simpleEnemy = _enemiesFactory.Create(Vector3.left * 100, shootingEnemyConfig, _station);*/

            TurretConfig turretConfig = _configsProvider.GetConfig<TurretConfig>();
            _turret = _entitiesFactory.CreateTurret(new Vector3(1, 1, 0) * 20, turretConfig);
            _brainsFactory.CreateTurretBrain(_turret, new NearestDamageableTargetSelector(_turret));

            BombConfig bombConfig = _configsProvider.GetConfig<BombConfig>();
            _entitiesFactory.CreateBomb(Vector3.right * 40, bombConfig);
        }

        private void Update()
        {
            if (_isRunning == false)
                return;

            if (_inputService.IsClicked)
            {
                if(_screenToWorldPointRaycastService.Raycast(out RaycastHit hit))
                    _station.StartAreaAttackRequest.Invoke(hit.point);
            }
        }
    }
}
