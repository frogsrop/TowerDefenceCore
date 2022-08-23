using Unity.Entities;

[GenerateAuthoringComponent]
public struct BulletSetPrefab : IComponentData
{
    public Entity Prefab;
}