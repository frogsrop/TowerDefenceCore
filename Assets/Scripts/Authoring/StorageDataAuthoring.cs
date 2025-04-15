using Unity.Entities;
using UnityEngine;

public class StorageDataAuthoring : MonoBehaviour
{
    public GameObject SimpleTowerPrefab;
    public GameObject FireTowerPrefab;
    public GameObject EnemyPrefab;
    public int Coins = 50;
}

class PayManagerBaker : Baker<StorageDataAuthoring>
{
    public override void Bake(StorageDataAuthoring authoring)
    {
        var storageEntity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(storageEntity, new StorageDataComponent
        {
            SimpleTowerPrefab = GetEntity(authoring.SimpleTowerPrefab, TransformUsageFlags.Dynamic),
            FireTowerPrefab = GetEntity(authoring.FireTowerPrefab, TransformUsageFlags.Dynamic),
            EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
            Coins = authoring.Coins
        });
    }
}