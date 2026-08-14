using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.OrbitMovement
{
    public static class OrbitGeometry
    {
        public const float FULL_CIRCLE_DEGREES = 360f;

        public static Vector3 GetPointOn(Vector3 center, float radius, float angleInDegrees)
        {
            float angleInRadians = angleInDegrees * Mathf.Deg2Rad;

            return center + new Vector3(Mathf.Cos(angleInRadians), 0, Mathf.Sin(angleInRadians)) * radius;
        }
    }
}
