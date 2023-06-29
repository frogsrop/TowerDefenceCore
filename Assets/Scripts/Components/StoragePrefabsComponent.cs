using Unity.Entities;
using UnityEngine;

public struct StoragePrefabsComponent : IComponentData
{
    public Entity TowerPrefab;
    public Entity EnemyPrefab;
}