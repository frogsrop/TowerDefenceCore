using Unity.Entities;
using Unity.Mathematics;

public struct PrefabComponent : IComponentData
{
    public Entity Prefab;
}