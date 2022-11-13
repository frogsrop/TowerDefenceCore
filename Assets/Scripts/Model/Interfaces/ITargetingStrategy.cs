using Unity.Entities;
using Unity.Mathematics;

public interface ITargetingStrategy
{
    Entity ChooseTarget(Entity tower, EntityQuery enemiesQuery);
}
