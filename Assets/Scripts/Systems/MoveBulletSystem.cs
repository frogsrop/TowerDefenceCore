using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class MoveBulletSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var queryTargetId = GetEntityQuery(ComponentType.ReadOnly<TargetIdComponent>());
        var queryEnemy = GetEntityQuery(ComponentType.ReadOnly<EnemyIdComponent>(),
            ComponentType.ReadOnly<LocalToWorldTransform>(),
            ComponentType.ReadOnly<DamageBufferElement>(),
            ComponentType.ReadOnly<BurningBufferElement>());
        var enemyIds = queryEnemy.ToComponentDataArray<EnemyIdComponent>(Allocator.TempJob);
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var dt = SystemAPI.Time.DeltaTime;
        if (enemyIds.Length <= 0) return;
        new MoveBulletJob
        {
            dtJob = dt,
            ecbJob = ecb,
            enemyIdsJob = enemyIds,
            enemyTransformsJob = queryEnemy.ToComponentDataArray<LocalToWorldTransform>(Allocator.TempJob),
            enemyEntityArrayJob = queryEnemy.ToEntityArray(Allocator.TempJob)
        }.Run(queryTargetId);
        Dependency.Complete();
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}

public partial struct MoveBulletJob : IJobEntity
{
    public float dtJob;
    public EntityCommandBuffer ecbJob;
    public NativeArray<EnemyIdComponent> enemyIdsJob;
    public NativeArray<LocalToWorldTransform> enemyTransformsJob;
    public NativeArray<Entity> enemyEntityArrayJob;

    private void Execute(ref LocalToWorldTransform bulletTransform, in TargetIdComponent bullet, in BulletComponent bulletInfo,
        in Entity entity)
    {
        var mapping = AbstractEffectConfig.Mapping;
        var enemyIndex = IndexOf(enemyIdsJob, bullet.Id);
        if (enemyIndex != -1)
        {
            var enemyTransform = enemyTransformsJob[enemyIndex];
            var enemyEntity = enemyEntityArrayJob[enemyIndex];
            var direction = math.normalize(enemyTransform.Value.Position - bulletTransform.Value.Position);
            bulletTransform.Value.Position += direction * dtJob * 10;
            var distance = math.distancesq(bulletTransform.Value.Position, enemyTransform.Value.Position);
            if (!(distance < 0.1f)) return;
            ecbJob.DestroyEntity(entity);
            foreach (var effect in bulletInfo.ListEffects)
            {
                if (mapping.ContainsKey(effect))
                {
                    mapping[effect].AppendToBuffer(enemyEntity, ecbJob);
                }
            }
        }
        else
        {
            ecbJob.DestroyEntity(entity);
        }
    }

    private int IndexOf(NativeArray<EnemyIdComponent> array, int id)
    {
        for (var i = 0; i < array.Length; i++)
        {
            if (array[i].Id == id) return i;
        }
        return -1;
    }
}