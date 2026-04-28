using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Configs.Gameplay.Stages;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.Enemies;
using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Gameplay.Features.StagesFeature
{
    public class ClearAllEnemiesStage : IStage
    {
        private readonly ClearAllEnemiesStageConfig _config;
        private readonly ReactiveEvent _completed = new ReactiveEvent();
        private readonly EnemiesFactory _enemiesFactory;
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private readonly Entity _enemiesTarget;
        private bool _inProcess;

        private readonly Dictionary<Entity, IDisposable> _spawnedEnemiesToRemoveReason = new Dictionary<Entity, IDisposable>();

        public ClearAllEnemiesStage(
            ClearAllEnemiesStageConfig config, 
            EnemiesFactory enemiesFactory, 
            Entity enemiesTarget, 
            EntitiesLifeContext entitiesLifeContext)
        {
            _config = config;
            _enemiesFactory = enemiesFactory;
            _enemiesTarget = enemiesTarget;
            _entitiesLifeContext = entitiesLifeContext;
        }

        public IReadOnlyEvent Completed => _completed;

        public void Start()
        {
            if (_inProcess)
                throw new InvalidOperationException("Game Mode Already Running");

            SpawnEnemies();

            _inProcess = true;
        }

        public void Update(float deltaTime)
        {
            if(_inProcess == false)
                return;

            if (_spawnedEnemiesToRemoveReason.Count == 0)
                ProcessEnd();
        }

        private void ProcessEnd()
        {
            _inProcess = false;
            _completed.Invoke();
        }

        public void Cleanup()
        {
            foreach (KeyValuePair<Entity, IDisposable> item in _spawnedEnemiesToRemoveReason)
            {
                item.Value.Dispose();
                _entitiesLifeContext.Release(item.Key);
            }

            _spawnedEnemiesToRemoveReason.Clear();
            _inProcess = true;
        }

        public void Dispose()
        {
            foreach (KeyValuePair<Entity, IDisposable> item in _spawnedEnemiesToRemoveReason)
                item.Value.Dispose();

            _spawnedEnemiesToRemoveReason.Clear();
            _inProcess = true;
        }

        private void SpawnEnemies()
        {
            foreach (EnemyItemConfig enemyItemConfig in _config.EnemyItems)
                SpawnEnemy(enemyItemConfig);
        }

        private void SpawnEnemy(EnemyItemConfig enemyItemConfig)
        {
            Entity spawnedEnemy = _enemiesFactory.Create(enemyItemConfig.SpawnPosition, enemyItemConfig.EnemyConfig, _enemiesTarget);
            IDisposable removeReason = spawnedEnemy.IsDead.Subscribe((oldValue, isDead) =>
            {
                if (isDead)
                {
                    IDisposable disposable = _spawnedEnemiesToRemoveReason[spawnedEnemy];
                    disposable.Dispose();
                    _spawnedEnemiesToRemoveReason.Remove(spawnedEnemy);
                }
            });

            _spawnedEnemiesToRemoveReason.Add(spawnedEnemy,  removeReason);
        }
    }
}
