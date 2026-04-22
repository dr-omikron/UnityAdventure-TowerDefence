using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.InputFeature;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.Physic;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay
{
    public class TestGameplay : MonoBehaviour
    {
        private DIContainer _container;
        private EntitiesFactory _entitiesFactory;
        private Entity _simpleEnemy;
        private Entity _station;
        private Entity _shootingEnemy;
        private bool _isRunning;

        private ScreenToWorldPointRaycastService _screenToWorldPointRaycastService;
        private IInputService _inputService;

        public void Initialize(DIContainer container)
        {
            _container = container;
            _entitiesFactory = _container.Resolve<EntitiesFactory>();
            _inputService = _container.Resolve<IInputService>();
            _screenToWorldPointRaycastService = _container.Resolve<ScreenToWorldPointRaycastService>();
        }

        public void Run()
        {
            _isRunning = true;
            _station = _entitiesFactory.CreateStation();
            _simpleEnemy = _entitiesFactory.CreateSimpleEnemy(Vector3.right * 50);
            _shootingEnemy = _entitiesFactory.CreateShootingEnemy(Vector3.left * 50);
            Entity simpleEnemy2 = _entitiesFactory.CreateSimpleEnemy(Vector3.forward * 50);
        }

        private void Update()
        {
            if (_isRunning == false)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
                _simpleEnemy.TakeDamageRequest.Invoke(50);

            if (Input.GetKeyDown(KeyCode.R))
                _shootingEnemy.StartAttackRequest.Invoke();

            if (_inputService.IsClicked)
            {
                if(_screenToWorldPointRaycastService.Raycast(out RaycastHit hit))
                    _station.StartAreaAttackRequest.Invoke(hit.point);
            }


            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            _simpleEnemy.MoveDirection.Value = input;
            _simpleEnemy.RotationDirection.Value = input;
        }
    }
}
