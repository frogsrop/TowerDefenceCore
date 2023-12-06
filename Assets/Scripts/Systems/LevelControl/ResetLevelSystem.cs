using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct ResetLevelSystem : ISystem
{
    private EntityQuery _queryStorage;
    private EntityQuery _queryOffScene;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _queryStorage = state.GetEntityQuery(
            ComponentType.ReadWrite<StorageStatusLevelComponent>());
        _queryOffScene = state.GetEntityQuery(
            ComponentType.ReadWrite<OffSceneComponent>());
    }
    
    public void OnUpdate(ref SystemState state)
    {
        var entityStorage = _queryStorage.GetSingletonEntity();
        var statusReset = state.EntityManager.GetComponentData<StorageStatusLevelComponent>(entityStorage).Reset;
        
        if (statusReset)
        {
            var simulationSystemGroup = state.EntityManager.World.GetExistingSystemManaged<SimulationSystemGroup>();
            simulationSystemGroup.Enabled = true;
            var statusLevel = new StorageStatusLevelComponent{ Stop = true};
            state.EntityManager.SetComponentData(entityStorage, statusLevel);
            state.EntityManager.DestroyEntity(_queryOffScene);
        }
    }

}
