using System.Collections;
using _Project.Develop.Runtime.Gameplay.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using _Project.Develop.Runtime.Utilities.LoadingScreen;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.Infrastructure.EntryPoint
{
    public class GameEntryPoint : MonoBehaviour
    {
        private void Awake()
        {
            //Данный проект шаблонный
            //В проект включен часто используемые сервисы и утилиты
            //Найти и чтобы при необходимости убрать сервисы проще всего в классах где происходит их регистрации:
            //ProjectContextRegistration, MainMenuContextRegistration, GameplayContextRegistration

            Debug.Log("Старт проекта, сетап настроек");
            SetupAppSettings();

            Debug.Log("Процесс регистрации всего проекта");

            DIContainer projectContainer = new DIContainer();
            ProjectContextRegistrations.Process(projectContainer);
            projectContainer.Initialize();

            projectContainer.Resolve<ICoroutinesPerformer>().StartPerform(Initialize(projectContainer));
        }

        private void SetupAppSettings()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }

        private IEnumerator Initialize(DIContainer container)
        {
            ILoadingScreen loadingScreen = container.Resolve<ILoadingScreen>();
            SceneSwitcherService sceneSwitcherService = container.Resolve<SceneSwitcherService>();
            PlayerDataProvider playerDataProvider = container.Resolve<PlayerDataProvider>();

            loadingScreen.Show();
            Debug.Log("Начинается инициализация сервисов");

            yield return container.Resolve<ConfigsProviderService>().LoadAsync();

            bool isPlayerDataSaveExist = false;

            yield return playerDataProvider.ExistsAsync(result => isPlayerDataSaveExist = result);

            if (isPlayerDataSaveExist)
                yield return playerDataProvider.LoadAsync();
            else
                playerDataProvider.Reset();

            yield return new WaitForSeconds(1.0f);

            Debug.Log("Завершается инициализация сервисов");
            loadingScreen.Hide();

            yield return sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu);
        }
    }
}
