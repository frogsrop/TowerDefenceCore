using Unity.Entities;

[GenerateAuthoringComponent]
public struct BulletSpawnAuthoring : IComponentData
{
    public Entity Prefab;
}