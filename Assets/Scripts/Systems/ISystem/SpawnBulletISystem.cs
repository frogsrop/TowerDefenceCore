using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class  SpawnBulletISystem : SystemBase
{
    private Timer _timer =new (4f);
    private Random _random;

    protected override void OnCreate()
     {
         _random.InitState();
     }

    protected override void OnUpdate()
    {
        var dt = SystemAPI.Time.DeltaTime;
        if (!_timer.refreshTimerAndCheckFinish(dt = dt))
        {
            return;
        }

        var queryEnemies = GetEntityQuery(ComponentType.ReadOnly<EnemyIdComponent>());
        var enemyIds = queryEnemies.ToComponentDataArray<EnemyIdComponent>(Allocator.TempJob);
        var enemyIdLinq = enemyIds[(int)(_random.NextUInt() % enemyIds.Length)];
        var enemyId = enemyIdLinq.Id;
        var towerQuery = GetEntityQuery(ComponentType.ReadOnly<Tower>());
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        new SpawnBulletJob
        {
            enemyId = enemyId,
            ecb = ecb
        }.Run(towerQuery);
        Dependency.Complete();
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}

public partial struct SpawnBulletJob : IJobEntity
{
    public int enemyId;
    public EntityCommandBuffer ecb;
    private void Execute(in LocalToWorldTransform towerTransform, in Tower tower)
    {
        var newBullet = ecb.Instantiate(tower.BulletPrefab);
        var towerFirePosition = new float3(towerTransform.Value.Position.x,
            towerTransform.Value.Position.y + 0.5f,
              towerTransform.Value.Position.z);
        var towerUniformScaleTransform = new UniformScaleTransform
            { Position = towerFirePosition, Scale = 0.2f };
        var setSpawnPosition = new LocalToWorldTransform
            { Value = towerUniformScaleTransform };
        ecb.SetComponent(newBullet, setSpawnPosition);
        var targetId = enemyId;
        ecb.AddComponent(newBullet, new TargetIdComponent { Id = targetId });
    }
}