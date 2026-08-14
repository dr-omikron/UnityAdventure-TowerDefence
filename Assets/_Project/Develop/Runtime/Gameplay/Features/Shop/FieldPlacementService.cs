using System;
using _Project.Develop.Runtime.Gameplay.Features.InputFeature;
using _Project.Develop.Runtime.Utilities.Physic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Develop.Runtime.Gameplay.Features.Shop
{
    public class FieldPlacementService
    {
        public event Action<Vector3> PositionPicked;

        private readonly IInputService _inputService;
        private readonly ScreenToWorldPointRaycastService _raycastService;

        private bool _isActive;

        public FieldPlacementService(IInputService inputService, ScreenToWorldPointRaycastService raycastService)
        {
            _inputService = inputService;
            _raycastService = raycastService;
        }

        public bool IsActive => _isActive;

        public void Begin() => _isActive = true;

        public void Cancel() => _isActive = false;

        public void Update(float deltaTime)
        {
            if (_isActive == false)
                return;

            if (_inputService.IsClicked == false)
                return;

            if (IsPointerOverUI())
                return;

            if (_raycastService.Raycast(out RaycastHit hit) == false)
                return;

            PositionPicked?.Invoke(hit.point);
        }

        private bool IsPointerOverUI()
        {
            if (EventSystem.current == null)
                return false;

            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}
