using _Project.Develop.Runtime.Utilities.Physic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Develop.Runtime.Gameplay.Features.InputFeature
{
    public class FieldClickService
    {
        private readonly IInputService _inputService;
        private readonly ScreenToWorldPointRaycastService _raycastService;

        public FieldClickService(IInputService inputService, ScreenToWorldPointRaycastService raycastService)
        {
            _inputService = inputService;
            _raycastService = raycastService;
        }

        public bool TryGetClickedPoint(out Vector3 point)
        {
            point = Vector3.zero;

            if (_inputService.IsClicked == false)
                return false;

            if (IsPointerOverUI())
                return false;

            if (_raycastService.Raycast(out RaycastHit hit) == false)
                return false;

            point = hit.point;
            return true;
        }

        private bool IsPointerOverUI()
        {
            if (EventSystem.current == null)
                return false;

            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}
