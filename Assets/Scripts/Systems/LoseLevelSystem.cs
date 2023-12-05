using Unity.Burst;
using Unity.Entities;

public partial struct LoseLevelSystem : ISystem
{
    private EntityQuery _queryStorage;

    public void OnCreate(ref SystemState state)
    {
        _queryStorage = state.GetEntityQuery(
            ComponentType.ReadWrite<StorageLevelHpComponent>());
    }

    public void OnUpdate(ref SystemState state)
    {
        var entityStorage = _queryStorage.GetSingletonEntity();
        var levelHp = state.EntityManager.GetComponentData<StorageLevelHpComponent>(entityStorage).LevelHp;
        if (levelHp <= 0)
        {
            var simulationSystemGroup = state.World.GetExistingSystemManaged<SimulationSystemGroup>();
            simulationSystemGroup.Enabled = false;
            var statusLevel = new StorageStatusLevelComponent { Lose = true, Stop = true};
            state.EntityManager.SetComponentData(entityStorage, statusLevel);
        }
    }
}