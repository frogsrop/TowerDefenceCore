using Unity.Entities;
using UnityEngine;

public struct StorageDataComponent : IComponentData
{
    public Entity SimpleTowerPrefab;
    public Entity FireTowerPrefab;
    public Entity EnemyPrefab;
    public int Coins;
}