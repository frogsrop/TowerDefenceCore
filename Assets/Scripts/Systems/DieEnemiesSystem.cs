using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct DieEnemiesSystem : ISystem
{
    private EntityQuery _queryEnemies;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _queryEnemies = state.GetEntityQuery(ComponentType.ReadWrite<EnemyHpComponent>());
    }
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        new DieEnemiesJob
        {
            Ecb = ecb
        }.Run(_queryEnemies);
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

[BurstCompile]
partial struct DieEnemiesJob : IJobEntity
{
    public EntityCommandBuffer Ecb;

    public void Execute(Entity entity, ref EnemyHpComponent enemyHp, in LocalToWorldTransform enemyTransform)
    {
        if (enemyHp.Hp <= 0)
        {
            Ecb.DestroyEntity(entity);
        }
    }
}
