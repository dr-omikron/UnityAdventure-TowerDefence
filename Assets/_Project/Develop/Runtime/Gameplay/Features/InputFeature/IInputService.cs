using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.InputFeature
{
    public interface IInputService
    {
        bool IsEnabled { get; set; }
        bool IsClicked { get; }
        Vector3 ScreenPosition { get; }
    }
}
