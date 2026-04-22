using _Project.Develop.Runtime.Gameplay.Features.InputFeature;
using UnityEngine;

namespace _Project.Develop.Runtime.Utilities.Physic
{
    public class ScreenToWorldPointRaycastService
    {
        private readonly Camera _camera = Camera.main;
        private readonly LayerMask _layerMask = 1 << LayerMask.NameToLayer("Ray");
        private readonly IInputService _inputService;

        public ScreenToWorldPointRaycastService(IInputService inputService)
        {
            _inputService = inputService;
        }

        public bool Raycast(out RaycastHit hit)
        {
            Ray ray = _camera.ScreenPointToRay(_inputService.ScreenPosition);
            return Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask.value);
        }
    }
}
