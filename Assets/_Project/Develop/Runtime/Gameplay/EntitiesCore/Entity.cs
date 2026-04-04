using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;

namespace _Project.Develop.Runtime.Gameplay.EntitiesCore
{
    public partial class Entity : IDisposable
    {
        public event Action<Entity> Initialized; 
        private readonly Dictionary<Type, IEntityComponent> _components = new Dictionary<Type, IEntityComponent>();
        private readonly List<IEntitySystem> _systems = new List<IEntitySystem>();

        private readonly List<IInitializableSystem> _initializable = new List<IInitializableSystem>();
        private readonly List<IUpdateableSystem> _updateable = new List<IUpdateableSystem>();
        private readonly List<IDisposableSystem> _disposable = new List<IDisposableSystem>();

        private bool _isInit;

        public bool IsInit => _isInit;

        public void Initialize()
        {
            foreach (IInitializableSystem initializable in _initializable)
                initializable.OnInit(this);

            _isInit = true;

            Initialized?.Invoke(this);
        }

        public void OnUpdate(float deltaTime)
        {
            if(_isInit == false)
                return;

            foreach (IUpdateableSystem updatable in _updateable)
                updatable.OnUpdate(deltaTime);
        }

        public void Dispose()
        {
            foreach (IDisposableSystem disposable in _disposable)
                disposable.OnDispose();

            _isInit = false;
        }

        public Entity AddComponent<TComponent>(TComponent component) where TComponent : class, IEntityComponent
        {
            _components.Add(typeof(TComponent), component);
            return this;
        }

        public bool HasComponent<TComponent>() where TComponent : class, IEntityComponent
            => _components.ContainsKey(typeof(TComponent));

        public bool TryGetComponent<TComponent>(out TComponent component) where TComponent : class, IEntityComponent
        {
            if (_components.TryGetValue(typeof(TComponent), out IEntityComponent foundObject))
            {
                component = (TComponent)foundObject;
                return true;
            }

            component = null;
            return false;
        }

        public TComponent GetComponent<TComponent>() where TComponent : class, IEntityComponent
        {
            if (TryGetComponent(out TComponent component) == false)
                throw new ArgumentException($"Entity not exist {typeof(TComponent)}");

            return component;
        }

        public Entity AddSystem(IEntitySystem system)
        {
            if(_systems.Contains(system))
                throw new ArgumentException($"Entity system already exist {system.GetType()}");

            _systems.Add(system);

            if(system is IInitializableSystem initializable)
            {
                _initializable.Add(initializable);

                if (_isInit)
                    initializable.OnInit(this);
            }

            if(system is IUpdateableSystem updateable)
                _updateable.Add(updateable);

            if(system is IDisposableSystem disposable)
                _disposable.Add(disposable);

            return this;
        }
    }
}
