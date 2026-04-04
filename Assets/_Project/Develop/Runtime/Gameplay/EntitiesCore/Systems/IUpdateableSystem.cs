namespace _Project.Develop.Runtime.Gameplay.EntitiesCore.Systems
{
    public interface IUpdateableSystem : IEntitySystem
    {
        void OnUpdate(float deltaTime);
    }
}
