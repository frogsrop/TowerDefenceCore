using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Random = Unity.Mathematics.Random;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
public partial struct TestResSystem : ISystem
{
    private Random _random;

    //[BurstCompile] 
    public void OnCreate(ref SystemState state)
    {
        _random.InitState();
    }
    [BurstCompile] public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        new SpawnEntitysJob { Ecb = ecb, Random = _random, Dt = dt}.Run();
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

public partial struct SpawnEntitysJob : IJobEntity
{
    public EntityCommandBuffer Ecb;
    public Random Random;
    public float Dt;
    private void Execute(Entity e, in ObjectsSpawnComponent objectsSpawnComponent, ref QuantitySpawnComponent quantitySpawn)
    {
        if(!quantitySpawn.OnOff) return;
        var timeSeed = (uint)(Dt * 100000);
        Random.InitState(timeSeed);
        var _minimumPosition = new float3(-8, -4, 0);
        var _maximumPosition = new float3(8, 2, 0);
        for (var i = 1; i <= quantitySpawn.QuantityTowers; i++)
        {
            var posTowerSpawn = Random.NextFloat3(_minimumPosition, _maximumPosition);
            var towerUniformScaleTransform = new UniformScaleTransform
            { Position = posTowerSpawn, Scale = 0.5f };
            var setSpawnTowerPosition = new LocalToWorldTransform
            { Value = towerUniformScaleTransform };
            var setSpeedAttack = new TowerSpeedAttack { Value = quantitySpawn.SpeedShoot };
            var _eTower = Ecb.Instantiate(objectsSpawnComponent.TowerPrefab);
            Ecb.SetComponent(_eTower, setSpawnTowerPosition);
            Ecb.SetComponent(_eTower, setSpeedAttack);

        }
        for (var i = 1; i <= quantitySpawn.QuantityEnemies; i++)
        {
            var posEnemySpawn = Random.NextFloat3(_minimumPosition, _maximumPosition);
            var enemyUniformScaleTransform = new UniformScaleTransform
            { Position = posEnemySpawn, Scale = 0.5f };
            var setSpawnEnemyPosition = new LocalToWorldTransform
            { Value = enemyUniformScaleTransform };
            var newEnemyId = new EnemyIdComponent { Id = i };
            var _eEnemy = Ecb.Instantiate(objectsSpawnComponent.EnemyPrefab);
            Ecb.SetComponent(_eEnemy, newEnemyId);
            Ecb.SetComponent(_eEnemy, setSpawnEnemyPosition);
        }
        var offSpawn = new QuantitySpawnComponent { OnOff = false };
        Ecb.SetComponent(e, offSpawn);
    }
}