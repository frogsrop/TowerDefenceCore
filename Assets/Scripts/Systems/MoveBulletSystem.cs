using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;

[BurstCompile]
public partial struct MoveBulletSystem : ISystem
{
    private EntityQuery _queryTargetId;
    private EntityQuery _queryEnemy;

    public void OnCreate(ref SystemState state)
    {
        var nativeArrayBullet = new NativeArray<ComponentType>(2, Allocator.Temp);
        nativeArrayBullet[0] = ComponentType.ReadOnly<BulletComponent>();
        nativeArrayBullet[1] = ComponentType.ReadOnly<TargetIdComponent>();
        _queryTargetId = state.GetEntityQuery(nativeArrayBullet);
        var nativeArrayEnemies = new NativeArray<ComponentType>(4, Allocator.Temp);
        nativeArrayEnemies[0] = ComponentType.ReadOnly<EnemyIdComponent>();
        nativeArrayEnemies[1] = ComponentType.ReadOnly<LocalToWorldTransform>();
        nativeArrayEnemies[2] = ComponentType.ReadOnly<DamageBufferElement>();
        nativeArrayEnemies[3] = ComponentType.ReadOnly<BurningBufferElement>();
        _queryEnemy = state.GetEntityQuery(nativeArrayEnemies);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var enemyIds = _queryEnemy.ToComponentDataArray<EnemyIdComponent>(Allocator.TempJob);
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var dt = SystemAPI.Time.DeltaTime;
        if (enemyIds.Length <= 0) return;
        new MoveBulletJob
        {
            Dt = dt,
            Ecb = ecb,
            EnemyIds = enemyIds,
            EnemyTransforms = _queryEnemy.ToComponentDataArray<LocalToWorldTransform>(Allocator.TempJob),
            EnemyEntityArray = _queryEnemy.ToEntityArray(Allocator.TempJob)
        }.Run(_queryTargetId);
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

public partial struct MoveBulletJob : IJobEntity
{
    public float Dt;
    public EntityCommandBuffer Ecb;
    public NativeArray<EnemyIdComponent> EnemyIds;
    public NativeArray<LocalToWorldTransform> EnemyTransforms;
    public NativeArray<Entity> EnemyEntityArray;

    private void Execute(ref LocalToWorldTransform bulletTransform, in TargetIdComponent bullet,
        in BulletComponent bulletInfo, in SpeedComponent bulletSpeed, in Entity entity)
    {
        var mapping = AbstractEffectConfig.Mapping; 
        var enemyIndex = IndexOf(EnemyIds, bullet.Id);
        if (enemyIndex != -1)
        {
            var enemyTransform = EnemyTransforms[enemyIndex];
            var enemyEntity = EnemyEntityArray[enemyIndex];
            var direction = math.normalize(enemyTransform.Value.Position - bulletTransform.Value.Position);
            bulletTransform.Value.Position += direction * Dt * bulletSpeed.Value;
            var distance = math.distancesq(bulletTransform.Value.Position, enemyTransform.Value.Position);
            if (!(distance < 0.1f)) return;
            Ecb.DestroyEntity(entity);
            foreach (var effect in bulletInfo.ListEffects)
            {
                if (mapping.ContainsKey(effect))
                {
                    mapping[effect].AppendToBuffer(enemyEntity, Ecb);
                }
            }
        }
        else
        {
            Ecb.DestroyEntity(entity);
        }
    }

    [BurstCompile]
    private int IndexOf(NativeArray<EnemyIdComponent> array, int id)
    {
        for (var i = 0; i < array.Length; i++)
        {
            if (array[i].Id == id) return i;
        }

        return -1;
    }
}