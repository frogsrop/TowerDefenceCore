using Unity.Entities;

public struct TestComponent : IComponentData
{
    public Entity TowerPrefab;
    public Entity EnemyPrefab;
}
