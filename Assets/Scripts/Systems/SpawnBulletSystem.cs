using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static UnityEngine.GraphicsBuffer;


public partial class SpawnBulletSystem : SystemBase
{
    private Random _random;
    private Timer _timer = new(4f);

    protected override void OnCreate()
    {
        _random.InitState();
    }

    protected override void OnUpdate()
    {
        if (!_timer.refreshTimerAndCheckFinish())
        {
            return;
        }

        var queryEnemies = GetEntityQuery(ComponentType.ReadOnly<EnemyIdComponent>());
        var enemyIds = queryEnemies.ToComponentDataArray<EnemyIdComponent>(Allocator.Temp);
        var enemyId = enemyIds[(int)(_random.NextUInt() % enemyIds.Length)];

        Entities.WithStructuralChanges().WithAll<Tower>().ForEach(
            (in LocalToWorldTransform towerTransform, in Tower tower) =>
            {
                
                var newBullet = EntityManager.Instantiate(tower.BulletPrefab);
                var towerFirePosition = new float3(towerTransform.Value.Position.x,
                    towerTransform.Value.Position.y + 0.5f,
                    towerTransform.Value.Position.z);
                var towerUniformScaleTransform = new UniformScaleTransform
                    { Position = towerFirePosition, Scale = 0.2f };
                var setSpawnPosition = new LocalToWorldTransform
                    { Value = towerUniformScaleTransform };
                EntityManager.SetComponentData(newBullet,
                    setSpawnPosition);
                var targetId = enemyId.Id;
                EntityManager.AddComponentData(newBullet, new TargetIdComponent { Id = targetId });
            }).Run();
    }
}