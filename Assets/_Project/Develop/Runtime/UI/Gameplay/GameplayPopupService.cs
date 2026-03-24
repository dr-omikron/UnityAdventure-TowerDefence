using System;
using _Project.Develop.Runtime.UI.Core;
using _Project.Develop.Runtime.UI.Gameplay.ResultPopups;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayPopupService : PopupService
    {
        private readonly GameplayUIRoot _uiRoot;
        private readonly GameplayPresentersFactory _gameplayPresentersFactory;

        public GameplayPopupService(ViewsFactory viewsFactory, 
            ProjectPresenterFactory presentersFactory, 
            GameplayUIRoot uiRoot, 
            GameplayPresentersFactory gameplayPresentersFactory) 
            : base(viewsFactory, presentersFactory)
        {
            _uiRoot = uiRoot;
            _gameplayPresentersFactory = gameplayPresentersFactory;
        }

        protected override Transform PopupLayer => _uiRoot.PopupsLayer;

        public WinPopupPresenter OpenWinPopup(Action closedCallback = null)
        {
            WinPopupView view = ViewsFactory.Create<WinPopupView>(ViewIDs.WinPopup, PopupLayer);
            WinPopupPresenter popupPresenter = _gameplayPresentersFactory.CreateWinPopupPresenter(view);
            OnPopupCreated(popupPresenter, view, closedCallback);
            return popupPresenter;
        }

        public DefeatPopupPresenter OpenDefeatPopup(Action closedCallback = null)
        {
            DefeatPopupView view = ViewsFactory.Create<DefeatPopupView>(ViewIDs.DefeatPopup, PopupLayer);
            DefeatPopupPresenter popupPresenter = _gameplayPresentersFactory.CreateDefeatPopupPresenter(view);
            OnPopupCreated(popupPresenter, view, closedCallback);
            return popupPresenter;
        }
    }
}
