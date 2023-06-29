using Unity.Entities;
using UnityEngine;

public class StoragePrefabsAuthoring : MonoBehaviour
{
    public GameObject TowerPrefab;
    public GameObject EnemyPrefab;
}

class PayManagerBaker : Baker<StoragePrefabsAuthoring>
{
    public override void Bake(StoragePrefabsAuthoring authoring)
    {
        var towerPrefab = GetEntity(authoring.TowerPrefab);
        var enemyPrefab = GetEntity(authoring.EnemyPrefab);

        AddComponent(new StoragePrefabsComponent
        {
            TowerPrefab = towerPrefab,
            EnemyPrefab = enemyPrefab
        });
    }
}