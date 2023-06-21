using Unity.Entities;
using UnityEngine;

public class StoragePrefabsAuthoring : MonoBehaviour
{
    public GameObject TowerPrefab;
    //public EnemyConfig enemyConfig;
    
}

class PayManagerBaker : Baker<StoragePrefabsAuthoring>
{

    public override void Bake(StoragePrefabsAuthoring authoring)
    {
        var TowerPrefab = GetEntity(authoring.TowerPrefab);

        AddComponent(new StoragePrefabsComponent
        {
            TowerPrefab = TowerPrefab
        });
        
        AddComponent(new StorageConfigsComponent
        {
            //EnemyConfig = enemyConfig
        });

    }
}