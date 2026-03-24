using _Project.Develop.Runtime.Gameplay.Infrastructure;
using _Project.Develop.Runtime.Meta.Features.LevelsProgression;
using _Project.Develop.Runtime.UI.Core;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.LevelMenuPopup
{
    public class LevelTilePresenter : ISubscribePresenter
    {
        private readonly LevelsProgressionService _levelsService;
        private readonly SceneSwitcherService _sceneSwitcher;
        private readonly ICoroutinesPerformer _coroutinesPerformer;

        private readonly int _levelNumber;
        private readonly LevelTileView _levelTileView;

        public LevelTilePresenter(
            LevelsProgressionService levelsService, 
            SceneSwitcherService sceneSwitcher, 
            ICoroutinesPerformer coroutinesPerformer, 
            int levelNumber, 
            LevelTileView levelTileView)
        {
            _levelsService = levelsService;
            _sceneSwitcher = sceneSwitcher;
            _coroutinesPerformer = coroutinesPerformer;
            _levelNumber = levelNumber;
            _levelTileView = levelTileView;
        }

        public LevelTileView LevelTileView => _levelTileView;

        public void Initialize()
        {
            _levelTileView.SetLevelNumber(_levelNumber.ToString());

            if (_levelsService.CanPlay(_levelNumber))
            {
                if(_levelsService.IsLevelCompleted(_levelNumber))
                    _levelTileView.SetComplete();
                else
                    _levelTileView.SetActive();
            }
            else
            {
                _levelTileView.SetBlock();
            }
        }

        public void Subscribe() => _levelTileView.Clicked += OnViewClicked;

        public void UnSubscribe() => _levelTileView.Clicked -= OnViewClicked;

        public void Dispose()
        {
            _levelTileView.Clicked -= OnViewClicked;
        }

        private void OnViewClicked()
        {
            if (_levelsService.CanPlay(_levelNumber) == false)
            {
                Debug.Log("Уровень заблокирован, пройдите предыдущий");
                return;
            }

            _coroutinesPerformer.StartPerform(
                _sceneSwitcher.ProcessSwitchTo(Scenes.Gameplay, new GameplayInputArgs(_levelNumber)));
        }

    }
}
