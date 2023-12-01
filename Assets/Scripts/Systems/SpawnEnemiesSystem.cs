using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct SpawnEnemiesSystem : ISystem
{
    private EntityQuery _querySpawners;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        var queries = new NativeArray<ComponentType>(4, Allocator.Temp);
        queries[0] = ComponentType.ReadOnly<SpawnerEnemiesComponent>();
        queries[1] = ComponentType.ReadOnly<SpawnCountEnemiesComponent>();
        queries[2] = ComponentType.ReadOnly<LocalTransform>();
        queries[3] = ComponentType.ReadOnly<TimerComponent>();
        _querySpawners = state.GetEntityQuery(queries);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        new SpawnJob { Ecb = ecb }.Run(_querySpawners);
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

//[BurstCompile]
partial struct SpawnJob : IJobEntity
{
    public EntityCommandBuffer Ecb;

    public void Execute(Entity entity, ref SpawnerEnemiesComponent spawnerEnemies,
        ref SpawnCountEnemiesComponent countEnemiesComponent, ref TimerComponent timerComponent,
        ref LocalTransform spawnerTransform)
    {
        if (!timerComponent.Condition)
            Ecb.SetComponent(entity, new TimerComponent
            {
                Condition = true, Trigger = false, Time = spawnerEnemies.SpeedSpawn, Delay = spawnerEnemies.SpeedSpawn
            });
        if (!timerComponent.Trigger) return;

        var newEnemy = Ecb.Instantiate(spawnerEnemies.EnemyPrefab);
        var enemySpawnPosition = new float3(spawnerTransform.Position.x + 0.5f,
            spawnerTransform.Position.y - 0.3f,
            spawnerTransform.Position.z);
        var setSpawnPosition = new LocalTransform
            { Position = enemySpawnPosition, Scale = 0.5f };

        Ecb.SetComponent(newEnemy, setSpawnPosition);
        var enemyId = new EnemyIdComponent { Id = countEnemiesComponent.Count };
        Ecb.SetComponent(newEnemy, enemyId);
        countEnemiesComponent.Count++;
    }
}