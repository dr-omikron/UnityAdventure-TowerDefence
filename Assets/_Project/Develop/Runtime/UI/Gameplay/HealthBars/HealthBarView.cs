using _Project.Develop.Runtime.UI.CommonViews;
using _Project.Develop.Runtime.UI.Core;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.Gameplay.HealthBars
{
    public class HealthBarView : MonoBehaviour, IView
    {
        [SerializeField] private BarWithText _barWithText;
        [SerializeField] private Vector3 _worldOffset = new Vector3(0, 5, 0);

        private Transform _target;
        private Camera _camera;

        public void SetTarget(Transform target)
        {
            _target = target;
            _camera = Camera.main;

            UpdatePosition();
        }

        public void UpdateValue(float currentHealth, float maxHealth)
        {
            _barWithText.UpdateSlider(currentHealth / maxHealth);
            _barWithText.UpdateText(currentHealth.ToString("0"));
        }

        private void LateUpdate()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (_target == null || _camera == null)
                return;

            transform.position = _camera.WorldToScreenPoint(_target.position + _worldOffset);
        }
    }
}
