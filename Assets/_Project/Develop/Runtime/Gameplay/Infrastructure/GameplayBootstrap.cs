using System;
using System.Collections;
using _Project.Develop.Runtime.Configs.Gameplay.Levels;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.AI;
using _Project.Develop.Runtime.Gameplay.Features.Station;
using _Project.Develop.Runtime.Gameplay.States;
using _Project.Develop.Runtime.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayBootstrap : SceneBootstrap
    {
        private DIContainer _container;
        private GameplayInputArgs _inputArgs;
        private EntitiesLifeContext _entitiesLifeContext;
        private AIBrainContext _aiBrainContext;
        private GameplayStatesContext _gameplayStatesContext;

        public override void ProcessRegistration(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            if (sceneArgs is not GameplayInputArgs gameplayInputArgs)
                throw new ArgumentException($"{nameof(sceneArgs)} is not match with {typeof(GameplayInputArgs)} type");

            _inputArgs = gameplayInputArgs;

            GameplayContextRegistrations.Process(_container, gameplayInputArgs);
        }

        public override IEnumerator Initialize()
        {
            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();
            _aiBrainContext = _container.Resolve<AIBrainContext>();
            _gameplayStatesContext = _container.Resolve<GameplayStatesContext>();

            _container.Resolve<StationFactory>()
                .Create(_container.Resolve<ConfigsProviderService>()
                    .GetConfig<LevelsListConfig>()
                    .GetBy(_inputArgs.LevelNumber));

            yield break;
        }

        public override void Run()
        {
            _gameplayStatesContext.Run();
        }

        private void Update()
        {
            _aiBrainContext?.Update(Time.deltaTime);
            _entitiesLifeContext?.Update(Time.deltaTime);
            _gameplayStatesContext?.Update(Time.deltaTime);
        }
    }
}
