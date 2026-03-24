using System.Collections.Generic;
using _Project.Develop.Runtime.Configs.Gameplay.Levels;
using _Project.Develop.Runtime.UI.Core;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;

namespace _Project.Develop.Runtime.UI.LevelMenuPopup
{
    public class LevelsMenuPopupPresenter : PopupPresenterBase
    {
        private const string TitleName = "LEVELS";

        private readonly ConfigsProviderService _configsProviderService;
        private readonly ProjectPresenterFactory _presenterFactory;
        private readonly ViewsFactory _viewsFactory;

        private readonly LevelsMenuPopupView _view;
        private readonly List<LevelTilePresenter> _levelTilePresenters = new List<LevelTilePresenter>();

        public LevelsMenuPopupPresenter(
            ICoroutinesPerformer coroutinesPerformer, 
            ConfigsProviderService configsProviderService, 
            ProjectPresenterFactory presenterFactory, 
            ViewsFactory viewsFactory, 
            LevelsMenuPopupView view) : base(coroutinesPerformer)
        {
            _configsProviderService = configsProviderService;
            _presenterFactory = presenterFactory;
            _viewsFactory = viewsFactory;
            _view = view;
        }

        protected override PopupViewBase PopupView => _view;

        public override void Initialize()
        {
            base.Initialize();
            _view.SetTitle(TitleName);
            LevelsListConfig levelsConfig = _configsProviderService.GetConfig<LevelsListConfig>();

            for (int i = 0; i < levelsConfig.Levels.Count; i++)
            {
                LevelTileView levelTileView = _viewsFactory.Create<LevelTileView>(ViewIDs.LevelsTile);
                _view.LevelTilesListView.Add(levelTileView);
                LevelTilePresenter levelTilePresenter = _presenterFactory.CreateLevelTilePresenter(levelTileView, i + 1);
                levelTilePresenter.Initialize();
                _levelTilePresenters.Add(levelTilePresenter);
            }
        }

        public override void Dispose()
        {
            foreach (LevelTilePresenter levelTilePresenter in _levelTilePresenters)
            {
                _view.LevelTilesListView.Remove(levelTilePresenter.LevelTileView);
                _viewsFactory.Release(levelTilePresenter.LevelTileView);
                levelTilePresenter.Dispose();
            }

            _levelTilePresenters.Clear();
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            foreach (LevelTilePresenter levelTilePresenter in _levelTilePresenters)
                levelTilePresenter.Subscribe();
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            foreach (LevelTilePresenter levelTilePresenter in _levelTilePresenters)
                levelTilePresenter.UnSubscribe();
        }
    }
}
