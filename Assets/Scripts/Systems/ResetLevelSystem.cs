using Unity.Entities;
using UnityEngine;

public partial class ResetLevelSystem : SystemBase
{
    private EntityQuery _queryStorage;
    private EntityQuery _queryOffScene;

    protected override void OnCreate()
    {
        _queryStorage = GetEntityQuery(
            ComponentType.ReadWrite<StorageStatusLevelComponent>());
        _queryOffScene = GetEntityQuery(
            ComponentType.ReadWrite<OffSceneComponent>());
    }
    
    protected override void OnUpdate()
    {
        var entityStorage = _queryStorage.GetSingletonEntity();
        var statusReset = EntityManager.GetComponentData<StorageStatusLevelComponent>(entityStorage).Reset;
        
        if (statusReset)
        {
            var simulationSystemGroup = EntityManager.World.GetExistingSystemManaged<SimulationSystemGroup>();
            simulationSystemGroup.Enabled = true;
            var statusLevel = new StorageStatusLevelComponent{ Stop = true};
            EntityManager.SetComponentData(entityStorage, statusLevel);
            EntityManager.DestroyEntity(_queryOffScene);
        }
    }

}
