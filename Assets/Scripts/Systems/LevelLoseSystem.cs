using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

public partial struct LevelLoseSystem : ISystem
{
    private EntityQuery _queryStorage;
    private EntityQuery _queryOffScene;

    public void OnCreate(ref SystemState state)
    {
        //var simulationSystemGroup = state.World.GetExistingSystemManaged<SimulationSystemGroup>();
        //simulationSystemGroup.Enabled = false; OffSceneComponent
        _queryStorage = state.GetEntityQuery(
            ComponentType.ReadWrite<StorageLevelHpComponent>());
        _queryOffScene = state.GetEntityQuery(
            ComponentType.ReadWrite<OffSceneComponent>());
    }

    public void OnUpdate(ref SystemState state)
    {
        //if (_queryStorage.GetSingletonEntity() == null) return;

        var entityStorage = _queryStorage.GetSingletonEntity();
        var levelHp = state.EntityManager.GetComponentData<StorageLevelHpComponent>(entityStorage).LevelHp;
        if (levelHp <= 0)
        {
            var simulationSystemGroup = state.World.GetExistingSystemManaged<SimulationSystemGroup>();
            state.EntityManager.DestroyEntity(_queryOffScene);
            simulationSystemGroup.Enabled = false;

            //state.World.DestroyAllSystemsAndLogException();
        }
    }
}