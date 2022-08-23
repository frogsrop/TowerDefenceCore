using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

public partial class MovingSystem : SystemBase
{
    private EntityQuery enemy;

    protected override void OnCreate()
    {
        enemy = GetEntityQuery(ComponentType.ReadOnly<CreepTag>(), ComponentType.ReadOnly<Translation>());
    }

    //[BurstCompile]
    protected override void OnUpdate()
    {
        var dt = Time.DeltaTime;
        var enemyTranslation = enemy.ToComponentDataArray<Translation>(Allocator.Temp)[0];
        Entities.WithAll<BulletTag>()
            .ForEach((ref Translation translation) =>
            {
                var direction = math.normalize(enemyTranslation.Value - translation.Value);
                translation.Value += direction * dt * 5;
            })
            //.WithBurst()
            .ScheduleParallel();
    }
}