using _Project.Develop.Runtime.Gameplay.EntitiesCore;
using _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using _Project.Develop.Runtime.Utilities;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Sensors
{
    public class BodyContactsDetectingSystem : IInitializableSystem, IUpdateableSystem
    {
        private const int X_AXIS_DIRECTION = 0;
        private const int Y_AXIS_DIRECTION = 1;

        private Buffer<Collider> _contacts;
        private LayerMask _layerMask;
        private CapsuleCollider _body;

        public void OnInit(Entity entity)
        {
            _contacts = entity.ContactCollidersBuffer;
            _layerMask = entity.ContactDetectingMask;
            _body = entity.BodyCollider;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_body.enabled == false)
            {
                _contacts.Count = 0;
                return;
            }

            GetWorldCapsule(out Vector3 startPoint, out Vector3 endPoint, out float radius);

            _contacts.Count = Physics.OverlapCapsuleNonAlloc(
                startPoint,
                endPoint,
                radius,
                _contacts.Items,
                _layerMask,
                QueryTriggerInteraction.Ignore);

            RemoveSelfFromContacts();
        }

        private void GetWorldCapsule(out Vector3 startPoint, out Vector3 endPoint, out float radius)
        {
            Transform bodyTransform = _body.transform;
            Vector3 scale = bodyTransform.lossyScale;

            Vector3 localAxis;
            float radiusScale;
            float heightScale;

            switch (_body.direction)
            {
                case X_AXIS_DIRECTION:
                    localAxis = Vector3.right;
                    radiusScale = Mathf.Max(Mathf.Abs(scale.y), Mathf.Abs(scale.z));
                    heightScale = Mathf.Abs(scale.x);
                    break;

                case Y_AXIS_DIRECTION:
                    localAxis = Vector3.up;
                    radiusScale = Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.z));
                    heightScale = Mathf.Abs(scale.y);
                    break;

                default:
                    localAxis = Vector3.forward;
                    radiusScale = Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.y));
                    heightScale = Mathf.Abs(scale.z);
                    break;
            }

            radius = _body.radius * radiusScale;

            float distanceToSphereCenter = Mathf.Max(_body.height * heightScale / 2 - radius, 0);

            Vector3 center = bodyTransform.TransformPoint(_body.center);
            Vector3 worldAxis = bodyTransform.TransformDirection(localAxis);

            startPoint = center + worldAxis * distanceToSphereCenter;
            endPoint = center - worldAxis * distanceToSphereCenter;
        }

        private void RemoveSelfFromContacts()
        {
            int indexToRemove = -1;

            for (int i = 0; i < _contacts.Count; i++)
            {
                if (_contacts.Items[i] == _body)
                {
                    indexToRemove = i;
                    break;
                }
            }

            if (indexToRemove >= 0)
            {
                for (int i = indexToRemove; i < _contacts.Count - 1; i++)
                {
                    _contacts.Items[i] = _contacts.Items[i + 1];
                }

                _contacts.Count--;
            }
        }
    }
}
