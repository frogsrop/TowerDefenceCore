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
        AddComponent(new StorageDataComponent
        {
            SimpleTowerPrefab = GetEntity(authoring.SimpleTowerPrefab),
            FireTowerPrefab = GetEntity(authoring.FireTowerPrefab),
            EnemyPrefab = GetEntity(authoring.EnemyPrefab),
            Coins = authoring.Coins
        });
    }
}