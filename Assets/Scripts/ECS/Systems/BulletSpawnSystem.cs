using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class BulletSpawnSystem : SystemBase
{
    private Entity bulletPrefab;
    private SpawnPosComponent spawnPosition;
    private float3 spawnPos;
    private EntityQuery enemy;

    protected override void OnCreate()
    {
        enemy = GetEntityQuery(ComponentType.ReadOnly<BulletSpawnTag>(), ComponentType.ReadOnly<Translation>());
    }
    protected override void OnStartRunning()
    {

        bulletPrefab = GetSingleton<BulletSpawnAuthoring>().Prefab;
    }

    protected override void OnUpdate()
    {
        var towerTranslation = enemy.ToComponentDataArray<Translation>(Allocator.Temp)[0];
        var newBullet = EntityManager.Instantiate(bulletPrefab);
        var setSpawnPosition = new Translation() { Value = towerTranslation.Value };

        EntityManager.SetComponentData(newBullet, setSpawnPosition);
        


        //EntityManager.DestroyEntity(newBullet);
    }
}
