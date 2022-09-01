/*using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class ShootingSystem : SystemBase
{
    private Entity bulletPrefab;
    private Entity newBullet;
    private EntityQuery queryTower;
    private EntityQuery queryBullet;
    private EntityQuery queryCreep;

    protected override void OnCreate()
    {
        queryTower = GetEntityQuery(ComponentType.ReadOnly<TowerTag>(), ComponentType.ReadOnly<Translation>());
        queryBullet = GetEntityQuery(ComponentType.ReadOnly<BulletTag>(), ComponentType.ReadOnly<Translation>());
        queryCreep = GetEntityQuery(ComponentType.ReadOnly<CreepTag>(), ComponentType.ReadOnly<Translation>());
    }

    protected override void OnStartRunning()
    {
        bulletPrefab = GetSingleton<BulletSetPrefab>().Prefab;
    }

    protected override void OnUpdate()
    {
        var bulletArray = queryBullet.ToComponentDataArray<BulletTag>(Allocator.Temp);
        if (bulletArray.Length < 1)
        {
            newBullet = EntityManager.Instantiate(bulletPrefab);
            var towerTranslation = queryTower.ToComponentDataArray<Translation>(Allocator.Temp)[0];
            var towerPosition = new float3(towerTranslation.Value.x, towerTranslation.Value.y + 0.5f,
                towerTranslation.Value.z);
            var setSpawnPosition = new Translation { Value = towerPosition };
            EntityManager.SetComponentData(newBullet, setSpawnPosition);
        }

        var firstBulletPos = queryBullet.ToComponentDataArray<Translation>(Allocator.Temp)[0];
        var firstCreepPos = queryCreep.ToComponentDataArray<Translation>(Allocator.Temp)[0];
        var distance = math.distancesq(firstBulletPos.Value, firstCreepPos.Value);
        if (distance < 0.1f)
        {
            EntityManager.DestroyEntity(newBullet);
        }
    }
}*/