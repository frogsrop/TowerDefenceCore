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
    private EntityQuery _queryStorage;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        var queriesEnemies = new NativeArray<ComponentType>(2, Allocator.Temp);
        queriesEnemies[0] = ComponentType.ReadOnly<DamageComponent>();
        queriesEnemies[1] = ComponentType.ReadOnly<EnemyHpComponent>();
        _queryEnemies = state.GetEntityQuery(queriesEnemies);
        _queryStorage = state.GetEntityQuery(
            ComponentType.ReadWrite<StorageCoinsComponent>());
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (_queryStorage.GetSingletonEntity() == null) return;
        
        var entityStorage = _queryStorage.GetSingletonEntity();
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var coins = state.EntityManager.GetComponentData<StorageCoinsComponent>(entityStorage).Coins;
        new DieEnemiesJob
        {
            Ecb = ecb,
            EntityStorage = entityStorage,
            CoinsBalance = coins
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
    public Entity EntityStorage;
    public int CoinsBalance;

    public void Execute(Entity entity, ref EnemyHpComponent enemyHp)
    {
        if (enemyHp.Hp <= 0)
        {
            Ecb.DestroyEntity(entity);
            var loot = new StorageCoinsComponent { Coins = CoinsBalance + 5 };
            Ecb.SetComponent(EntityStorage, loot);
        }
    }
}