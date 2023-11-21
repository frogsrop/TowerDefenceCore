using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct MoveEnemiesSystem : ISystem
{
    private EntityQuery _queryEnemies;
    private EntityQuery _queryCastles;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        var queryEnemies = new NativeArray<ComponentType>(4, Allocator.Temp);
        queryEnemies[0] = ComponentType.ReadOnly<LocalTransform>();
        queryEnemies[1] = ComponentType.ReadOnly<DirectionComponent>();
        queryEnemies[2] = ComponentType.ReadOnly<TargetIdComponent>();
        queryEnemies[3] = ComponentType.ReadOnly<SpeedComponent>();
        _queryEnemies = state.GetEntityQuery(queryEnemies);
        var queryCastles = new NativeArray<ComponentType>(3, Allocator.Temp);
        queryCastles[0] = ComponentType.ReadWrite<CastleComponent>();
        queryCastles[1] = ComponentType.ReadOnly<LocalTransform>();
        queryCastles[2] = ComponentType.ReadOnly<WayPointsComponent>();
        _queryCastles = state.GetEntityQuery(queryCastles);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(Allocator.TempJob);

        var castleEntityArray = _queryCastles.ToEntityArray(Allocator.TempJob);
        var dynamicBuffer = state.EntityManager.GetBuffer<WayPointsComponent>(castleEntityArray[0]);

        var floatArray = new NativeArray<float3>(dynamicBuffer.Length, Allocator.TempJob);
        for (int i = 0; i < dynamicBuffer.Length; i++)
        {
            floatArray[i] = dynamicBuffer[i].Value;
        }

        new MoveEnemyJob
        {
            Dt = dt,
            Ecb = ecb,
            PathArray = floatArray,
            CastleTransforms = _queryCastles.ToComponentDataArray<LocalTransform>(Allocator.TempJob),
            CastleHealth = _queryCastles.ToComponentDataArray<CastleComponent>(Allocator.TempJob),
            CastleEntityArray = castleEntityArray
        }.Run(_queryEnemies);
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

//[BurstCompile]
partial struct MoveEnemyJob : IJobEntity
{
    public float Dt;
    public EntityCommandBuffer Ecb;
    public NativeArray<float3> PathArray;
    public NativeArray<LocalTransform> CastleTransforms;
    public NativeArray<CastleComponent> CastleHealth;
    public NativeArray<Entity> CastleEntityArray;

    public void Execute(Entity entity, ref LocalTransform transform, ref DirectionComponent dir,
        ref TargetIdComponent target, ref SpeedComponent speed)
    {
        // for test Scene
        // if ((transform.Position.x > 5 && dir.Direction > 0) ||
        //     (transform.Position.x < -5 && dir.Direction < 0))
        // {
        //     dir.Direction *= -1;
        // }

        if (target.Id < PathArray.Length)
        {
            var direction = math.normalize(PathArray[target.Id] - transform.Position);
            transform.Position += direction * Dt * speed.Value;
            var distance = math.distancesq(transform.Position, PathArray[target.Id]);
            if (distance < 0.1) target.Id++;
        }
        else
        {
            var castleTransform = CastleTransforms[0];
            var direction = math.normalize(castleTransform.Position - transform.Position);
            transform.Position += direction * Dt * speed.Value;
            var distance = math.distancesq(transform.Position, castleTransform.Position);
            if (distance < 0.1)
            {
                var castleHealth = new CastleComponent { PassedEnemies = CastleHealth[0].PassedEnemies + 1 };
                Ecb.SetComponent(CastleEntityArray[0], castleHealth);
                Ecb.DestroyEntity(entity);
            }
        }
    }
}
// old path system

// [BurstCompile]
// public partial struct MoveEnemySystem : ISystem
// {
//     public void OnCreate(ref SystemState state)
//     {
//     }
//
//     public void OnDestroy(ref SystemState state)
//     {
//     }
//
//     [BurstCompile]
//     public void OnUpdate(ref SystemState state)
//     {
//         foreach (var pathFollower in SystemAPI.Query<PathFollowerAspect>())
//         {
//             pathFollower.FollowPath(SystemAPI.Time.DeltaTime);
//         }
//     }
// }