using _Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using _Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using _Project.Develop.Runtime.Gameplay.Features.AreaTakeDamage;
using _Project.Develop.Runtime.Gameplay.Features.Attack;
using _Project.Develop.Runtime.Gameplay.Features.Attack.AreaAttack;
using _Project.Develop.Runtime.Gameplay.Features.Attack.Shoot;
using _Project.Develop.Runtime.Gameplay.Features.ContactTakeDamage;
using _Project.Develop.Runtime.Gameplay.Features.LifeCycle;
using _Project.Develop.Runtime.Gameplay.Features.MovementFeatures;
using _Project.Develop.Runtime.Gameplay.Features.Sensors;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities;
using _Project.Develop.Runtime.Utilities.Conditions;
using _Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.EntitiesCore
{
    public class EntitiesFactory
    {
        private readonly DIContainer _container;
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private readonly MonoEntityFactory _monoEntityFactory;
        private readonly ContactsEntitiesFilterService _contactsEntitiesFilterService;

        public EntitiesFactory(DIContainer container)
        {
            _container = container;
            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();
            _monoEntityFactory = _container.Resolve<MonoEntityFactory>();
            _contactsEntitiesFilterService = _container.Resolve<ContactsEntitiesFilterService>();
        }

        public Entity CreateStation()
        {
            Entity entity = CreateEmpty();

            _monoEntityFactory.Create(entity, Vector3.zero, "Entities/Station");

            entity
                .AddIsStation()
                .AddMaxHealth(new ReactiveVariable<float>(100))
                .AddCurrentHealth(new ReactiveVariable<float>(100))
                .AddIsDead()
                .AddIsDeathProcess()
                .AddDeathProcessInitialTime(new ReactiveVariable<float>(2))
                .AddDeathProcessCurrentTime()

                .AddTakeDamageRequest()
                .AddTakeDamageEvent()

                .AddContactDetectingMask(1 << LayerMask.NameToLayer("Entity") | 1 << LayerMask.NameToLayer("Enemy"))
                .AddContactCollidersBuffer(new Buffer<Collider>(64))
                .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                .AddBodyContactDamage(new ReactiveVariable<float>(100))

                .AddAttackProcessInitialTime(new ReactiveVariable<float>(2))
                .AddAttackProcessCurrentTime()
                .AddInAttackProcess()
                .AddStartAreaAttackRequest()
                .AddStartAttackEvent()
                .AddEndAttackEvent()

                .AddCastAreaPositionEvent()
                .AddEndCastAreaPositionEvent()
                .AddAreaDetectingRadius(new ReactiveVariable<float>(30))
                .AddAreaDetectingMask(1 << LayerMask.NameToLayer("Enemy"))
                .AddAreaCollidersBuffer(new Buffer<Collider>(64))
                .AddAreaEntitiesBuffer(new Buffer<Entity>(64))
                .AddAreaDamage(new ReactiveVariable<float>(100));

            ICompositeCondition canApplyDamage = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.CurrentHealth.Value <= 0));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value))
                .Add(new FuncCondition(() => entity.IsDeathProcess.Value == false));

            ICompositeCondition canStartAttack = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false))
                .Add(new FuncCondition(() => entity.InAttackProcess.Value == false));

            entity
                .AddCanStartAttack(canStartAttack)
                .AddCanApplyDamage(canApplyDamage)
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease);

            entity
                .AddSystem(new BodyContactsDetectingSystem())
                .AddSystem(new BodyContactsEntitiesFilterSystem(_contactsEntitiesFilterService))
                .AddSystem(new DealDamageOnContactSystem())

                .AddSystem(new StartAreaAttackSystem())
                .AddSystem(new AttackProcessTimerSystem())
                .AddSystem(new EndAttackSystem())

                .AddSystem(new AriaContactsDetectingSystem())
                .AddSystem(new AreaContactsEntitiesFilterSystem(_contactsEntitiesFilterService))
                .AddSystem(new DealDamageOnAreaSystem())

                .AddSystem(new ApplyDamageSystem())
                .AddSystem(new DeathSystem())
                .AddSystem(new DisableCollidersOnDeathSystem())
                .AddSystem(new DeathProcessTimerSystem())
                .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        public Entity CreateSimpleEnemy(Vector3 position)
        {
            Entity entity = CreateEmpty();

            _monoEntityFactory.Create(entity, position, "Entities/SimpleEnemy");

            entity
                .AddIsMoving()
                .AddMoveDirection(new ReactiveVariable<Vector3>())
                .AddMoveSpeed(new ReactiveVariable<float>(50))
                .AddRotationDirection()
                .AddRotationSpeed(new ReactiveVariable<float>(900))
                .AddMaxHealth(new ReactiveVariable<float>(100))
                .AddCurrentHealth(new ReactiveVariable<float>(100))
                .AddIsDead()
                .AddIsDeathProcess()
                .AddDeathProcessInitialTime(new ReactiveVariable<float>(2))
                .AddDeathProcessCurrentTime()
                .AddTakeDamageRequest()
                .AddTakeDamageEvent()
                .AddContactDetectingMask(1 << LayerMask.NameToLayer("Entity"))
                .AddContactCollidersBuffer(new Buffer<Collider>(64))
                .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                .AddBodyContactDamage(new ReactiveVariable<float>(100));

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canRotate = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canApplyDamage = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.CurrentHealth.Value <= 0));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value))
                .Add(new FuncCondition(() => entity.IsDeathProcess.Value == false));

            entity
                .AddCanMove(canMove)
                .AddCanRotate(canRotate)
                .AddCanApplyDamage(canApplyDamage)
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease);

            entity
                .AddSystem(new RigidbodyMovementSystem())
                .AddSystem(new RigidbodyRotationSystem())
                .AddSystem(new BodyContactsDetectingSystem())
                .AddSystem(new BodyContactsEntitiesFilterSystem(_contactsEntitiesFilterService))
                .AddSystem(new DealDamageOnContactSystem())
                .AddSystem(new ApplyDamageSystem())
                .AddSystem(new DeathSystem())
                .AddSystem(new DisableCollidersOnDeathSystem())
                .AddSystem(new DeathProcessTimerSystem())
                .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        public Entity CreateShootingEnemy(Vector3 position)
        {
            Entity entity = CreateEmpty();

            _monoEntityFactory.Create(entity, position, "Entities/ShootingEnemy");

            entity
                .AddIsMoving()
                .AddMoveDirection(new ReactiveVariable<Vector3>())
                .AddMoveSpeed(new ReactiveVariable<float>(50))
                .AddRotationDirection()
                .AddRotationSpeed(new ReactiveVariable<float>(900))

                .AddAttackProcessInitialTime(new ReactiveVariable<float>(2))
                .AddAttackProcessCurrentTime()
                .AddInAttackProcess()
                .AddStartAttackRequest()
                .AddStartAttackEvent()
                .AddEndAttackEvent()

                .AddShootAttackDamage(new ReactiveVariable<float>(25))

                .AddMaxHealth(new ReactiveVariable<float>(100))
                .AddCurrentHealth(new ReactiveVariable<float>(100))

                .AddIsDead()
                .AddIsDeathProcess()
                .AddDeathProcessInitialTime(new ReactiveVariable<float>(2))
                .AddDeathProcessCurrentTime()

                .AddTakeDamageRequest()
                .AddTakeDamageEvent();

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canRotate = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canApplyDamage = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.CurrentHealth.Value <= 0));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value))
                .Add(new FuncCondition(() => entity.IsDeathProcess.Value == false));

            ICompositeCondition canStartAttack = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false))
                .Add(new FuncCondition(() => entity.InAttackProcess.Value == false));

            entity
                .AddCanMove(canMove)
                .AddCanRotate(canRotate)
                .AddCanApplyDamage(canApplyDamage)
                .AddMustDie(mustDie)
                .AddCanStartAttack(canStartAttack)
                .AddMustSelfRelease(mustSelfRelease);

            entity
                .AddSystem(new RigidbodyMovementSystem())
                .AddSystem(new RigidbodyRotationSystem())

                .AddSystem(new StartAttackSystem())
                .AddSystem(new AttackProcessTimerSystem())
                .AddSystem(new EndAttackSystem())

                .AddSystem(new InstantShootSystem())

                .AddSystem(new ApplyDamageSystem())
                .AddSystem(new DeathSystem())
                .AddSystem(new DisableCollidersOnDeathSystem())
                .AddSystem(new DeathProcessTimerSystem())
                .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        private Entity CreateEmpty() => new Entity();
    }
}
