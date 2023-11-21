using Unity.Entities;
using UnityEngine;

public class StorageDataAuthoring : MonoBehaviour
{
    public GameObject SimpleTowerPrefab;
    public GameObject FireTowerPrefab;
    public GameObject EnemyPrefab;
    public int Coins = 50;
    public int LevelHp = 10;
}

class PayManagerBaker : Baker<StorageDataAuthoring>
{
    public override void Bake(StorageDataAuthoring authoring)
    {
        var storageEntity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(storageEntity, new StorageEntitiesComponent
        {
            SimpleTowerPrefab = GetEntity(authoring.SimpleTowerPrefab, TransformUsageFlags.Dynamic),
            FireTowerPrefab = GetEntity(authoring.FireTowerPrefab, TransformUsageFlags.Dynamic),
            EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic)
        });
        AddComponent(storageEntity, new StorageLevelHpComponent
        {
            LevelHp = authoring.LevelHp
        });
        AddComponent(storageEntity, new StorageCoinsComponent
        {
            Coins = authoring.Coins
        });
    }
}