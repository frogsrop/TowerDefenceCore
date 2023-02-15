using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class SpawnBulletSystem : SystemBase
{
    private Random _random;

    protected override void OnCreate()
    {
        _random.InitState();
    }

    protected override void OnUpdate()
    {
        var queryEnemies = GetEntityQuery(ComponentType.ReadOnly<EnemyIdComponent>());
        var enemyIds = queryEnemies.ToComponentDataArray<EnemyIdComponent>(Allocator.TempJob);
        var enemyIdLinq = enemyIds[(int)(_random.NextUInt() % enemyIds.Length)];
        var enemyId = enemyIdLinq.Id;
        var towerQuery = GetEntityQuery(ComponentType.ReadOnly<Tower>());
        var ecb = new EntityCommandBuffer(Allocator.TempJob);

        new SpawnBulletJob { EnemyId = enemyId, Ecb = ecb }.Run(towerQuery);
        Dependency.Complete();
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}

public partial struct SpawnBulletJob : IJobEntity
{
    public int EnemyId;
    public EntityCommandBuffer Ecb;
    private void Execute(Entity entity, ref TimerComponent timerComponent, in LocalToWorldTransform towerTransform, in Tower tower)
    {
        if (!timerComponent.Condition) Ecb.SetComponent(entity,
            new TimerComponent { Condition = true, Trigger = false, Time = 4f, Delay = 4f });
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