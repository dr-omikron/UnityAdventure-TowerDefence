namespace _Project.Develop.Runtime.Gameplay.EntitiesCore
{
	public partial class Entity
	{
		public _Project.Develop.Runtime.Gameplay.Features.MovementFeatures.MoveDirection MoveDirectionC => GetComponent<_Project.Develop.Runtime.Gameplay.Features.MovementFeatures.MoveDirection>();

		public _Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<UnityEngine.Vector3> MoveDirection => MoveDirectionC.Value;

		public bool TryGetMoveDirection(out _Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<UnityEngine.Vector3> value)
		{
			bool result = TryGetComponent(out _Project.Develop.Runtime.Gameplay.Features.MovementFeatures.MoveDirection component);
			if(result)
				value = component.Value;
			else
				value = default(_Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<UnityEngine.Vector3>);
			return result;
		}

		public _Project.Develop.Runtime.Gameplay.EntitiesCore.Entity AddMoveDirection()
		{
			return AddComponent(new _Project.Develop.Runtime.Gameplay.Features.MovementFeatures.MoveDirection { Value = new _Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<UnityEngine.Vector3>() });
		}

		public _Project.Develop.Runtime.Gameplay.EntitiesCore.Entity AddMoveDirection(_Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<UnityEngine.Vector3> value)
		{
			return AddComponent(new _Project.Develop.Runtime.Gameplay.Features.MovementFeatures.MoveDirection {Value = value});
		}

		public _Project.Develop.Runtime.Gameplay.Features.MovementFeatures.MoveSpeed MoveSpeedC => GetComponent<_Project.Develop.Runtime.Gameplay.Features.MovementFeatures.MoveSpeed>();

		public _Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<System.Single> MoveSpeed => MoveSpeedC.Value;

		public bool TryGetMoveSpeed(out _Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<System.Single> value)
		{
			bool result = TryGetComponent(out _Project.Develop.Runtime.Gameplay.Features.MovementFeatures.MoveSpeed component);
			if(result)
				value = component.Value;
			else
				value = default(_Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<System.Single>);
			return result;
		}

		public _Project.Develop.Runtime.Gameplay.EntitiesCore.Entity AddMoveSpeed()
		{
			return AddComponent(new _Project.Develop.Runtime.Gameplay.Features.MovementFeatures.MoveSpeed { Value = new _Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<System.Single>() });
		}

		public _Project.Develop.Runtime.Gameplay.EntitiesCore.Entity AddMoveSpeed(_Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<System.Single> value)
		{
			return AddComponent(new _Project.Develop.Runtime.Gameplay.Features.MovementFeatures.MoveSpeed {Value = value});
		}

		public _Project.Develop.Runtime.Gameplay.Features.MovementFeatures.RotationDirection RotationDirectionC => GetComponent<_Project.Develop.Runtime.Gameplay.Features.MovementFeatures.RotationDirection>();

		public _Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<UnityEngine.Vector3> RotationDirection => RotationDirectionC.Value;

		public bool TryGetRotationDirection(out _Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<UnityEngine.Vector3> value)
		{
			bool result = TryGetComponent(out _Project.Develop.Runtime.Gameplay.Features.MovementFeatures.RotationDirection component);
			if(result)
				value = component.Value;
			else
				value = default(_Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<UnityEngine.Vector3>);
			return result;
		}

		public _Project.Develop.Runtime.Gameplay.EntitiesCore.Entity AddRotationDirection()
		{
			return AddComponent(new _Project.Develop.Runtime.Gameplay.Features.MovementFeatures.RotationDirection { Value = new _Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<UnityEngine.Vector3>() });
		}

		public _Project.Develop.Runtime.Gameplay.EntitiesCore.Entity AddRotationDirection(_Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<UnityEngine.Vector3> value)
		{
			return AddComponent(new _Project.Develop.Runtime.Gameplay.Features.MovementFeatures.RotationDirection {Value = value});
		}

		public _Project.Develop.Runtime.Gameplay.Features.MovementFeatures.RotationSpeed RotationSpeedC => GetComponent<_Project.Develop.Runtime.Gameplay.Features.MovementFeatures.RotationSpeed>();

		public _Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<System.Single> RotationSpeed => RotationSpeedC.Value;

		public bool TryGetRotationSpeed(out _Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<System.Single> value)
		{
			bool result = TryGetComponent(out _Project.Develop.Runtime.Gameplay.Features.MovementFeatures.RotationSpeed component);
			if(result)
				value = component.Value;
			else
				value = default(_Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<System.Single>);
			return result;
		}

		public _Project.Develop.Runtime.Gameplay.EntitiesCore.Entity AddRotationSpeed()
		{
			return AddComponent(new _Project.Develop.Runtime.Gameplay.Features.MovementFeatures.RotationSpeed { Value = new _Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<System.Single>() });
		}

		public _Project.Develop.Runtime.Gameplay.EntitiesCore.Entity AddRotationSpeed(_Project.Develop.Runtime.Utilities.Reactive.ReactiveVariable<System.Single> value)
		{
			return AddComponent(new _Project.Develop.Runtime.Gameplay.Features.MovementFeatures.RotationSpeed {Value = value});
		}

		public _Project.Develop.Runtime.Gameplay.Common.RigidbodyComponent RigidbodyC => GetComponent<_Project.Develop.Runtime.Gameplay.Common.RigidbodyComponent>();

		public UnityEngine.Rigidbody Rigidbody => RigidbodyC.Value;

		public bool TryGetRigidbody(out UnityEngine.Rigidbody value)
		{
			bool result = TryGetComponent(out _Project.Develop.Runtime.Gameplay.Common.RigidbodyComponent component);
			if(result)
				value = component.Value;
			else
				value = default(UnityEngine.Rigidbody);
			return result;
		}

		public _Project.Develop.Runtime.Gameplay.EntitiesCore.Entity AddRigidbody(UnityEngine.Rigidbody value)
		{
			return AddComponent(new _Project.Develop.Runtime.Gameplay.Common.RigidbodyComponent {Value = value});
		}

	}
}
