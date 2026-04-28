using _Project.Develop.Runtime.Configs.Gameplay.Entities;
using _Project.Develop.Runtime.Configs.Gameplay.Levels;
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
using _Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
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

        public Entity CreateStation(StationConfig stationConfig, LevelConfig levelConfig)
        {
            Entity entity = CreateEmpty();

            _monoEntityFactory.Create(entity, Vector3.zero, stationConfig.PrefabPath);

            entity
                .AddMaxHealth(new ReactiveVariable<float>(levelConfig.StationMaxHealth))
                .AddCurrentHealth(new ReactiveVariable<float>(levelConfig.StationMaxHealth))
                .AddIsDead()
                .AddIsDeathProcess()
                .AddDeathProcessInitialTime(new ReactiveVariable<float>(stationConfig.DeathProcessTime))
                .AddDeathProcessCurrentTime()

                .AddTakeDamageRequest()
                .AddTakeDamageEvent()

                .AddContactDetectingMask(Layers.EntityMask)
                .AddContactCollidersBuffer(new Buffer<Collider>(64))
                .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                .AddBodyContactDamage(new ReactiveVariable<float>(1000))

                .AddAttackProcessInitialTime(new ReactiveVariable<float>(stationConfig.AttackProcessInitialTime))
                .AddAttackProcessCurrentTime()
                .AddInAttackProcess()
                .AddStartAreaAttackRequest()
                .AddStartAttackEvent()
                .AddEndAttackEvent()
                .AddAttackCanceledEvent()
                .AddAttackCooldownCurrentTime()
                .AddAttackCooldownInitialTime(new ReactiveVariable<float>(stationConfig.AttackCooldown))
                .AddInAttackCooldown()

                .AddCastAreaPositionEvent()
                .AddEndCastAreaPositionEvent()
                .AddAreaDetectingRadius(new ReactiveVariable<float>(stationConfig.AreaDamageRadius))
                .AddAreaDetectingMask(Layers.EntityMask)
                .AddAreaCollidersBuffer(new Buffer<Collider>(64))
                .AddAreaEntitiesBuffer(new Buffer<Entity>(64))
                .AddAreaDamage(new ReactiveVariable<float>(stationConfig.AreaDamage));

            ICompositeCondition canApplyDamage = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.CurrentHealth.Value <= 0));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value))
                .Add(new FuncCondition(() => entity.IsDeathProcess.Value == false));

            ICompositeCondition canStartAttack = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false))
                .Add(new FuncCondition(() => entity.InAttackProcess.Value == false))
                .Add(new FuncCondition(() => entity.InAttackCooldown.Value == false));

            ICompositeCondition mustCanceledAttack = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value));

            entity
                .AddCanStartAttack(canStartAttack)
                .AddCanApplyDamage(canApplyDamage)
                .AddMustDie(mustDie)
                .AddMustCanceledAttack(mustCanceledAttack)
                .AddMustSelfRelease(mustSelfRelease);

            entity
                .AddSystem(new BodyContactsDetectingSystem())
                .AddSystem(new BodyContactsEntitiesFilterSystem(_contactsEntitiesFilterService))
                .AddSystem(new DealDamageOnContactSystem())

                .AddSystem(new StartAreaAttackSystem())
                .AddSystem(new AttackCanceledSystem())
                .AddSystem(new AttackProcessTimerSystem())
                .AddSystem(new EndAttackSystem())
                .AddSystem(new AttackCooldownTimerSystem())

                .AddSystem(new AriaContactsDetectingSystem())
                .AddSystem(new AreaContactsEntitiesFilterSystem(_contactsEntitiesFilterService))
                .AddSystem(new DealDamageOnAreaSystem())

                .AddSystem(new ApplyDamageSystem())
                .AddSystem(new DeathSystem())
                .AddSystem(new DisableCollidersOnDeathSystem())
                .AddSystem(new DeathProcessTimerSystem())
                .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            return entity;
        }

        public Entity CreateSimpleEnemy(Vector3 position, ReactiveVariable<Entity> currentTarget, SimpleEnemyConfig simpleEnemyConfig)
        {
            Entity entity = CreateEmpty();

            _monoEntityFactory.Create(entity, position, simpleEnemyConfig.PrefabPath);

            entity
                .AddCurrentTarget(currentTarget)
                .AddIsMoving()
                .AddMoveDirection(new ReactiveVariable<Vector3>())
                .AddMoveSpeed(new ReactiveVariable<float>(simpleEnemyConfig.MoveSpeed))
                .AddRotationDirection()
                .AddRotationSpeed(new ReactiveVariable<float>(simpleEnemyConfig.RotationSpeed))
                .AddMaxHealth(new ReactiveVariable<float>(simpleEnemyConfig.MaxHealth))
                .AddCurrentHealth(new ReactiveVariable<float>(simpleEnemyConfig.MaxHealth))
                .AddIsDead()
                .AddIsDeathProcess()
                .AddDeathProcessInitialTime(new ReactiveVariable<float>(simpleEnemyConfig.DeathProcessTime))
                .AddDeathProcessCurrentTime()
                .AddStartAttackDistance(new ReactiveVariable<float>(1))
                .AddTakeDamageRequest()
                .AddTakeDamageEvent()
                .AddContactDetectingMask(Layers.EntityMask)
                .AddContactCollidersBuffer(new Buffer<Collider>(64))
                .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                .AddBodyContactDamage(new ReactiveVariable<float>(simpleEnemyConfig.BodyContactDamage));

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

            return entity;
        }

        public Entity CreateShootingEnemy(Vector3 position, ReactiveVariable<Entity> currentTarget, ShootingEnemyConfig shootingEnemyConfig)
        {
            Entity entity = CreateEmpty();

            _monoEntityFactory.Create(entity, position, shootingEnemyConfig.PrefabPath);

            entity
                .AddCurrentTarget(currentTarget)
                .AddIsMoving()
                .AddMoveDirection(new ReactiveVariable<Vector3>())
                .AddMoveSpeed(new ReactiveVariable<float>(shootingEnemyConfig.MoveSpeed))
                .AddRotationDirection()
                .AddRotationSpeed(new ReactiveVariable<float>(shootingEnemyConfig.RotationSpeed))

                .AddAttackProcessInitialTime(new ReactiveVariable<float>(shootingEnemyConfig.AttackProcessInitialTime))
                .AddAttackProcessCurrentTime()
                .AddInAttackProcess()
                .AddStartAttackRequest()
                .AddStartAttackEvent()
                .AddEndAttackEvent()
                .AddAttackCanceledEvent()
                .AddAttackCooldownCurrentTime()
                .AddAttackCooldownInitialTime(new ReactiveVariable<float>(shootingEnemyConfig.AttackCooldown))
                .AddInAttackCooldown()

                .AddShootAttackDamage(new ReactiveVariable<float>(shootingEnemyConfig.AttackDamage / 2))
                .AddStartAttackDistance(new ReactiveVariable<float>(shootingEnemyConfig.StartAttackDistance))

                .AddMaxHealth(new ReactiveVariable<float>(shootingEnemyConfig.MaxHealth))
                .AddCurrentHealth(new ReactiveVariable<float>(shootingEnemyConfig.MaxHealth))

                .AddIsDead()
                .AddIsDeathProcess()
                .AddDeathProcessInitialTime(new ReactiveVariable<float>(shootingEnemyConfig.DeathProcessTime))
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
                .Add(new FuncCondition(() => entity.InAttackProcess.Value == false))
                .Add(new FuncCondition(() => entity.InAttackCooldown.Value == false));

            ICompositeCondition mustCanceledAttack = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value));

            entity
                .AddCanMove(canMove)
                .AddCanRotate(canRotate)
                .AddCanApplyDamage(canApplyDamage)
                .AddMustDie(mustDie)
                .AddCanStartAttack(canStartAttack)
                .AddMustCanceledAttack(mustCanceledAttack)
                .AddMustSelfRelease(mustSelfRelease);

            entity
                .AddSystem(new RigidbodyMovementSystem())
                .AddSystem(new RigidbodyRotationSystem())

                .AddSystem(new StartAttackSystem())
                .AddSystem(new AttackCanceledSystem())
                .AddSystem(new AttackProcessTimerSystem())
                .AddSystem(new EndAttackSystem())
                .AddSystem(new AttackCooldownTimerSystem())

                .AddSystem(new InstantShootSystem(this))

                .AddSystem(new ApplyDamageSystem())
                .AddSystem(new DeathSystem())
                .AddSystem(new DisableCollidersOnDeathSystem())
                .AddSystem(new DeathProcessTimerSystem())
                .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            return entity;
        }

        public Entity CreateProjectile(Vector3 position, Vector3 direction, float damage, Entity owner)
        {
            Entity entity = CreateEmpty();

            _monoEntityFactory.Create(entity, position, "Entities/EnemyProjectile");

            entity
                .AddIsMoving()
                .AddMoveDirection(new ReactiveVariable<Vector3>(direction))
                .AddMoveSpeed(new ReactiveVariable<float>(100))
                .AddRotationDirection(new ReactiveVariable<Vector3>(direction))
                .AddRotationSpeed(new ReactiveVariable<float>(9999))
                .AddIsDead()
                .AddContactDetectingMask(Layers.EntityMask | Layers.Environment)
                .AddContactCollidersBuffer(new Buffer<Collider>(64))
                .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                .AddBodyContactDamage(new ReactiveVariable<float>(damage))
                .AddDeathMask(Layers.Environment)
                .AddIsTouchingDeathMask()
                .AddIsTouchAnotherTeam()
                .AddTeam(new ReactiveVariable<Teams>(owner.Team.Value));

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canRotate = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => entity.IsTouchingDeathMask.Value))
                .Add(new FuncCondition(() => entity.IsTouchAnotherTeam.Value));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value));

            entity
                .AddCanMove(canMove)
                .AddCanRotate(canRotate)
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease);

            entity
                .AddSystem(new RigidbodyMovementSystem())
                .AddSystem(new RigidbodyRotationSystem())
                .AddSystem(new BodyContactsDetectingSystem())
                .AddSystem(new BodyContactsEntitiesFilterSystem(_contactsEntitiesFilterService))
                .AddSystem(new DealDamageOnContactSystem())
                .AddSystem(new DeathMaskTouchDetectorSystem())
                .AddSystem(new AnotherTeamTouchDetectorSystem())
                .AddSystem(new DeathSystem())
                .AddSystem(new DisableCollidersOnDeathSystem())
                .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        public Entity CreateTurret(Vector3 position, TurretConfig turretConfig)
        {
            Entity entity = CreateEmpty();

            _monoEntityFactory.Create(entity, position, turretConfig.PrefabPath);

            entity
                .AddCurrentTarget()
                .AddRotationDirection(new ReactiveVariable<Vector3>())
                .AddRotationSpeed(new ReactiveVariable<float>(turretConfig.RotationSpeed))

                .AddAttackProcessInitialTime(new ReactiveVariable<float>(turretConfig.AttackProcessInitialTime))
                .AddAttackProcessCurrentTime()
                .AddInAttackProcess()
                .AddStartAttackRequest()
                .AddStartAttackEvent()
                .AddEndAttackEvent()
                .AddAttackCanceledEvent()
                .AddAttackCooldownCurrentTime()
                .AddAttackCooldownInitialTime(new ReactiveVariable<float>(turretConfig.AttackCooldown))
                .AddInAttackCooldown()

                .AddShootAttackDamage(new ReactiveVariable<float>(turretConfig.AttackDamage / 2))
                .AddTeam(new ReactiveVariable<Teams>(Teams.Station));

            ICompositeCondition canRotate = new CompositeCondition()
                .Add(new FuncCondition(() => true));

            ICompositeCondition canStartAttack = new CompositeCondition()
                .Add(new FuncCondition(() => entity.InAttackProcess.Value == false))
                .Add(new FuncCondition(() => entity.InAttackCooldown.Value == false));

            ICompositeCondition mustCanceledAttack = new CompositeCondition()
                .Add(new FuncCondition(() => false));

            entity
                .AddCanRotate(canRotate)
                .AddCanStartAttack(canStartAttack)
                .AddMustCanceledAttack(mustCanceledAttack);

            entity
                .AddSystem(new RigidbodyRotationSystem())
                .AddSystem(new StartAttackSystem())
                .AddSystem(new AttackCanceledSystem())
                .AddSystem(new AttackProcessTimerSystem())
                .AddSystem(new EndAttackSystem())
                .AddSystem(new AttackCooldownTimerSystem())
                .AddSystem(new InstantShootSystem(this));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        public Entity CreateBomb(Vector3 position, BombConfig bombConfig)
        {
            Entity entity = CreateEmpty();

            _monoEntityFactory.Create(entity, position, bombConfig.PrefabPath);

            entity
                .AddIsDead()
                .AddContactDetectingMask(Layers.EntityMask)
                .AddContactCollidersBuffer(new Buffer<Collider>(64))
                .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                .AddBodyContactDamage(new ReactiveVariable<float>(bombConfig.BodyContactDamage))
                .AddDeathMask(Layers.EntityMask)
                .AddIsTouchingDeathMask()
                .AddIsTouchAnotherTeam()
                .AddTeam(new ReactiveVariable<Teams>(Teams.Station));

            ICompositeCondition mustDie = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => entity.IsTouchingDeathMask.Value))
                .Add(new FuncCondition(() => entity.IsTouchAnotherTeam.Value));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value));

            entity
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease);

            entity
                .AddSystem(new BodyContactsDetectingSystem())
                .AddSystem(new BodyContactsEntitiesFilterSystem(_contactsEntitiesFilterService))
                .AddSystem(new DealDamageOnContactSystem())
                .AddSystem(new DeathMaskTouchDetectorSystem())
                .AddSystem(new AnotherTeamTouchDetectorSystem())
                .AddSystem(new DeathSystem())
                .AddSystem(new DisableCollidersOnDeathSystem())
                .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        private Entity CreateEmpty() => new Entity();
    }
}
