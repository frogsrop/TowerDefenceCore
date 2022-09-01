using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class MoveToTargetSystem : SystemBase
{
    private EntityQuery queryBullet;
    private EntityQuery queryCreep;
    
    protected override void OnUpdate()

    {
        queryBullet = GetEntityQuery(ComponentType.ReadOnly<BulletTag>(), ComponentType.ReadOnly<Translation>(),
            ComponentType.ReadOnly<IDEnemy>());   //список пуль
        queryCreep = GetEntityQuery(ComponentType.ReadOnly<CreepTag>(), ComponentType.ReadOnly<Translation>(),
            ComponentType.ReadOnly<IDEnemy>()); //список крипов

        var dt = Time.DeltaTime;
        var bulletID = queryBullet.ToComponentDataArray<IDEnemy>(Allocator.Temp)[0];
        var creepID = queryCreep.ToComponentDataArray<IDEnemy>(Allocator.Temp)[0];
        var creepTranslation = queryCreep.ToComponentDataArray<Translation>(Allocator.Temp)[0];
        if (bulletID.value == creepID.value)
        {
            Entities.WithAll<BulletTag>()
            .ForEach((ref Translation translation) =>
            {
                var direction = math.normalize(creepTranslation.Value - translation.Value);
                translation.Value += direction * dt * 7;
            })
            //.WithBurst()
            .ScheduleParallel();
        }


    }
}
