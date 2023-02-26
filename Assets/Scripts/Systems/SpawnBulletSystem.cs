using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

//[BurstCompile]
public partial struct SpawnBulletSystem : ISystem
{
    

    private Random _random;
    private EntityQuery _queryEnemies;
    private EntityQuery _towerQuery;

    //[BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _random.InitState();
        _queryEnemies = state.GetEntityQuery(ComponentType.ReadOnly<EnemyIdComponent>());
        _towerQuery = state.GetEntityQuery(ComponentType.ReadOnly<Tower>());
    }
    // [BurstCompile] 
    public void OnDestroy(ref SystemState state) { }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var enemyIds = _queryEnemies.ToComponentDataArray<EnemyIdComponent>(Allocator.TempJob);
        if (enemyIds.Length == 0) return;
        var enemyIdLinq = enemyIds[(int)(_random.NextUInt() % enemyIds.Length)];
        var enemyId = enemyIdLinq.Id;
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        new SpawnBulletJob { EnemyId = enemyId, Ecb = ecb }.Run(_towerQuery);
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

//[BurstCompile]
public partial struct SpawnBulletJob : IJobEntity
{
    public int EnemyId;
    public EntityCommandBuffer Ecb;

    //[BurstCompile]
    private void Execute(Entity entity, ref TimerComponent timerComponent, 
        in LocalToWorldTransform towerTransform, in Tower tower, in TowerSpeedAttack towerSpeedAttack)
    {
        if (!timerComponent.Condition) Ecb.SetComponent(entity, new TimerComponent { 
            Condition = true, Trigger = false, Time = towerSpeedAttack.Value, Delay = towerSpeedAttack.Value });
        if (!timerComponent.Trigger) return;
        var newBullet = Ecb.Instantiate(tower.BulletPrefab);
        var towerFirePosition = new float3(towerTransform.Value.Position.x,
            towerTransform.Value.Position.y + 0.5f,
              towerTransform.Value.Position.z);
        var towerUniformScaleTransform = new UniformScaleTransform
        { Position = towerFirePosition, Scale = 0.2f };
        var setSpawnPosition = new LocalToWorldTransform
        { Value = towerUniformScaleTransform };
        Ecb.SetComponent(newBullet, setSpawnPosition);
        var targetId = EnemyId;
        Ecb.AddComponent(newBullet, new TargetIdComponent { Id = targetId });
    }
}