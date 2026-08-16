using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.Common;
using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.Features.LifeCycle;
using _Project.Develop.Runtime.Gameplay.Features.Station;
using _Project.Develop.Runtime.UI.Core;
using UnityEngine;

namespace _Project.Develop.Runtime.UI.Gameplay.HealthBars
{
    public class EntitiesHealthBarsPresenter : IPresenter
    {
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private readonly ViewsFactory _viewsFactory;
        private readonly GameplayPresentersFactory _presentersFactory;
        private readonly Transform _healthBarsParent;

        private readonly Dictionary<Entity, HealthBarPresenter> _entityToPresenter =
            new Dictionary<Entity, HealthBarPresenter>();

        public EntitiesHealthBarsPresenter(
            EntitiesLifeContext entitiesLifeContext,
            ViewsFactory viewsFactory,
            GameplayPresentersFactory presentersFactory,
            Transform healthBarsParent)
        {
            _entitiesLifeContext = entitiesLifeContext;
            _viewsFactory = viewsFactory;
            _presentersFactory = presentersFactory;
            _healthBarsParent = healthBarsParent;
        }

        public void Initialize()
        {
            _entitiesLifeContext.Added += OnEntityAdded;
            _entitiesLifeContext.Released += OnEntityReleased;

            foreach (Entity entity in _entitiesLifeContext.Entities)
                TryCreateHealthBarFor(entity);
        }

        public void Dispose()
        {
            _entitiesLifeContext.Added -= OnEntityAdded;
            _entitiesLifeContext.Released -= OnEntityReleased;

            foreach (HealthBarPresenter presenter in _entityToPresenter.Values)
                ReleasePresenter(presenter);

            _entityToPresenter.Clear();
        }

        private void OnEntityAdded(Entity entity) => TryCreateHealthBarFor(entity);

        private void OnEntityReleased(Entity entity)
        {
            if (_entityToPresenter.TryGetValue(entity, out HealthBarPresenter presenter) == false)
                return;

            ReleasePresenter(presenter);
            _entityToPresenter.Remove(entity);
        }

        private void TryCreateHealthBarFor(Entity entity)
        {
            if (HasHealthBar(entity) == false)
                return;

            if (_entityToPresenter.ContainsKey(entity))
                return;

            HealthBarView healthBarView = _viewsFactory
                .Create<HealthBarView>(GetViewIDFor(entity), _healthBarsParent);

            HealthBarPresenter presenter = _presentersFactory.CreateHealthBarPresenter(healthBarView, entity);
            presenter.Initialize();

            _entityToPresenter.Add(entity, presenter);
        }

        private bool HasHealthBar(Entity entity)
        {
            return entity.HasComponent<CurrentHealth>()
                   && entity.HasComponent<MaxHealth>()
                   && entity.HasComponent<TransformComponent>();
        }

        private string GetViewIDFor(Entity entity)
        {
            if (entity.HasComponent<IsStation>())
                return ViewIDs.StationHealthBar;

            return ViewIDs.SimpleHealthBar;
        }

        private void ReleasePresenter(HealthBarPresenter presenter)
        {
            _viewsFactory.Release(presenter.HealthBarView);
            presenter.Dispose();
        }
    }
}
