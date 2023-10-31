using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


[BurstCompile]
public partial struct SpawnEnemiesSystem : ISystem
{
    private EntityQuery _querySpawners;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _querySpawners = state.GetEntityQuery(ComponentType.ReadWrite<SpawnerEnemiesComponent>());
    }
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        new TestSpawnJob { Ecb = ecb }.Run(_querySpawners);
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

[BurstCompile]
partial struct TestSpawnJob : IJobEntity
{
    public EntityCommandBuffer Ecb;

    public void Execute(Entity entity, ref SpawnerEnemiesComponent spawnerEnemies, ref SpawnCountEnemiesComponent countEnemiesComponent,ref TimerComponent timerComponent, 
        in LocalToWorldTransform spawnerTransform)
    {
        if (!timerComponent.Condition)
            Ecb.SetComponent(entity, new TimerComponent
            {
                Condition = true, Trigger = false, Time = spawnerEnemies.SpeedSpawn, Delay = spawnerEnemies.SpeedSpawn
            });
        if (!timerComponent.Trigger) return;
        
        var newEnemy = Ecb.Instantiate(spawnerEnemies.EnemyPrefab);
        var enemySpawnPosition = new float3(spawnerTransform.Value.Position.x+ 0.5f,
            spawnerTransform.Value.Position.y,
            spawnerTransform.Value.Position.z);
        var enemySpawnUniformScaleTransform = new UniformScaleTransform
            { Position = enemySpawnPosition, Scale = 0.2f };
        var setSpawnPosition = new LocalToWorldTransform
            { Value = enemySpawnUniformScaleTransform };
        
      
        Ecb.SetComponent(newEnemy, setSpawnPosition);
        var enemyId = new EnemyIdComponent { Id = countEnemiesComponent.Count };
        Ecb.SetComponent(newEnemy, enemyId);
        countEnemiesComponent.Count++;
    }
}
