using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class DestroyBulletSystem : SystemBase
{
    private Entity newBullet;
    private EntityQuery queryBullet;
    private EntityQuery queryCreep;
    protected override void OnUpdate()
    {
        queryBullet = GetEntityQuery(ComponentType.ReadOnly<BulletTag>(), ComponentType.ReadOnly<Translation>(),
            ComponentType.ReadOnly<IDEnemy>());   //список пуль
        queryCreep = GetEntityQuery(ComponentType.ReadOnly<CreepTag>(), ComponentType.ReadOnly<Translation>(),
            ComponentType.ReadOnly<IDEnemy>()); //список крипов
        newBullet = queryBullet.ToEntityArray(Allocator.Temp)[0];

        var firstBulletPos = queryBullet.ToComponentDataArray<Translation>(Allocator.Temp)[0];
        var firstCreepPos = queryCreep.ToComponentDataArray<Translation>(Allocator.Temp)[0];
        var distance = math.distancesq(firstBulletPos.Value, firstCreepPos.Value);
        if (distance < 0.1f)
        {
            EntityManager.DestroyEntity(newBullet);
        }
    }
}