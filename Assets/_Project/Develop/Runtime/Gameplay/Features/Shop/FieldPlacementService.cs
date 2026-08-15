using System;
using _Project.Develop.Runtime.Gameplay.Features.InputFeature;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Shop
{
    public class FieldPlacementService
    {
        public event Action<Vector3> PositionPicked;

        private readonly FieldClickService _fieldClickService;

        private bool _isActive;

        public FieldPlacementService(FieldClickService fieldClickService)
        {
            _fieldClickService = fieldClickService;
        }

        public bool IsActive => _isActive;

        public void Begin() => _isActive = true;

        public void Cancel() => _isActive = false;

        public void Update(float deltaTime)
        {
            if (_isActive == false)
                return;

            if (_fieldClickService.TryGetClickedPoint(out Vector3 point) == false)
                return;

            PositionPicked?.Invoke(point);
        }
    }
}
