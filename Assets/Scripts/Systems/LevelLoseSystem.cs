using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

public partial struct LevelLoseSystem : ISystem
{
    private EntityQuery _queryStorage;
    private Entity _entityStorage;

    public void OnCreate(ref SystemState state)
    {
        _queryStorage = state.GetEntityQuery(
            ComponentType.ReadWrite<StorageLevelHpComponent>());
    }

    public void OnUpdate(ref SystemState state)
    {
        _entityStorage = _queryStorage.GetSingletonEntity();
        var levelHp = state.EntityManager.GetComponentData<StorageLevelHpComponent>(_entityStorage).LevelHp;
        if (levelHp <= 0)
        {
            var simulationSystemGroup = state.World.GetExistingSystemManaged<SimulationSystemGroup>();
            simulationSystemGroup.Enabled = false;
        }

        
    }
}