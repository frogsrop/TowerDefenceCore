using Unity.Collections;
using Unity.Entities;
using Unity.Burst;

[BurstCompile]
public partial struct DestroySystem : ISystem
{
    private EntityQuery _queryDestroy;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        var query = new NativeArray<ComponentType>(1, Allocator.Temp);
        query[0] = ComponentType.ReadOnly<DestroyComponent>();
        _queryDestroy = state.GetEntityQuery(query);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        new DestroyJob
        {
            Ecb = ecb,
        }.Run(_queryDestroy);
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
        
    }
}

[BurstCompile]
public partial struct DestroyJob : IJobEntity
{
    public EntityCommandBuffer Ecb;
    [BurstCompile]
    private void Execute(in Entity entity)
    {
        Ecb.DestroyEntity(entity);
    }
}