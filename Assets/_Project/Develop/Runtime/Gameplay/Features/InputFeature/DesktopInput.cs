using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.InputFeature
{
    public class DesktopInput : IInputService
    {
        public bool IsEnabled { get; set; } = true;
        public bool IsClicked => Input.GetMouseButtonDown(0);
        public Vector3 ScreenPosition => Input.mousePosition;
    }
}
