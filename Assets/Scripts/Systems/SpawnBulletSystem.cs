using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


public partial class SpawnBulletSystem : SystemBase
{
    private Random _random;
    private Timer timer = new(0.5f);

    protected override void OnCreate()
    {
        _random.InitState();
    }

    protected override void OnUpdate()
    {
        if (!timer.refreshTimerAndCheckFinish())
        {
            return;
        }

        var queryEnemies = GetEntityQuery(ComponentType.ReadOnly<Enemy>(), ComponentType.ReadOnly<IDEnemy>());
        var enemyIds = queryEnemies.ToComponentDataArray<IDEnemy>(Allocator.Temp);
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
                var enemyID = enemyId.Id;
                EntityManager.AddComponentData(newBullet, new IDBullet { Id = enemyID });
            }).Run();
    }
}