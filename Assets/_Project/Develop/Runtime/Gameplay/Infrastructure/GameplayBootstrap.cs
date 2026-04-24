using System;
using System.Collections;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.AI;
using _Project.Develop.Runtime.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
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

        [SerializeField] private TestGameplay _testGameplay;

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
            Debug.Log("Loaded level number: " + _inputArgs.LevelNumber);
            Debug.Log("Gameplay Scene Initialized");

            _testGameplay.Initialize(_container);

            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();
            _aiBrainContext = _container.Resolve<AIBrainContext>();
            //_gameplayStatesContext = _container.Resolve<GameplayStatesContext>();

            yield break;
        }

        public override void Run()
        {
            Debug.Log("Gameplay Scene Started");

            _testGameplay.Run();

            //_gameplayStatesContext.Run();
        }

        private void Update()
        {
            _aiBrainContext?.Update(Time.deltaTime);
            _entitiesLifeContext?.Update(Time.deltaTime);
            //_gameplayStatesContext?.Update(Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
                ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();
                coroutinesPerformer.StartPerform(sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));
            }
        }
    }
}
