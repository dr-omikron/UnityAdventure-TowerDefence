using _Project.Develop.Runtime.UI.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.LevelMenuPopup
{
    public class LevelsMenuPopupView : PopupViewBase
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private LevelTilesListView _levelTilesListView;

        public LevelTilesListView LevelTilesListView => _levelTilesListView;
        public void SetTitle(string title) => _title.text = title;

        protected override void ModifyShowAnimation(Sequence sequence)
        {
            base.ModifyShowAnimation(sequence);

            foreach (LevelTileView levelTileView in _levelTilesListView.Elements)
            {
                sequence.Append(levelTileView.Show());
                sequence.AppendInterval(0.1f);
            }
        }
    }
}
