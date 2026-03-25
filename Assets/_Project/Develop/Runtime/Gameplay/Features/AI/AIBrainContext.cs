using System.Collections.Generic;

namespace _Project.Develop.Runtime.Gameplay.Features.AI
{
    public class AIBrainContext
    {
        //private readonly List<EntityToBrain> _entityToBrains = new List<EntityToBrain>();

        /*private class EntityToBrain(Entity entity, IBrain brain)
        {
            public Entity Entity;
            public IBrain Brain;

            public EntityToBrain(Entity entity, IBrain brain)
            {
                Entity = entity;
                Brain = brain;
            }
        }*/

        /*public void SetFor(Entity entity, IBrain brain)
        {
            foreach (EntityToBrain entityToBrain in _entitiesToBrain)
            {
                if (entity == entityToBrain.Entity)
                {
                    entityToBrain.Brain.Disable();
                    entityToBrain.Brain.Dispose();
                    entityToBrain.Brain = brain;
                    entityToBrain.Brain.Enable();

                    return;
                }
            }

            _entityToBrains.Add(new EntityToBrain(entity, brain));
            brain.Enable();
        }*/

        /*public void Update(float deltaTime)
        {
            for (int i = 0; i < _entitiesToBrain.Count; i++)
            {
                if (_entitiesToBrain[i].Entity.IsInit == false)
                {
                    int lastIndex = _entitiesToBrain.Count - 1;
                    
                    _entitiesToBrain[i].Brain.Dispose();
                    _entitiesToBrain[i] = _entitiesToBrain[lastIndex];
                    _entitiesToBrain.RemoveAt(lastIndex);
                    i--;
                    continue;
                }

                _entitiesToBrain[i].Brain.Update(deltaTime);
            }
        }*/

        /*public void Dispose()
        {
            foreach (EntityToBrain entityToBrain in _entitiesToBrain)
                entityToBrain.Brain.Dispose();

            _entitiesToBrain.Clear();
        }*/
    }
}
