using Unity.Entities;

public struct ObjectsSpawnComponent : IComponentData
{
    public Entity TowerPrefab;
    public Entity EnemyPrefab;
}