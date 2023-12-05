using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public partial struct VictoryLevelSystem : ISystem
{
    private EntityQuery _queryStorage;
    private EntityQuery _queryEnemies;

    public void OnCreate(ref SystemState state)
    {
        _queryStorage = state.GetEntityQuery(ComponentType.ReadWrite<StorageLevelHpComponent>());
        _queryEnemies = state.GetEntityQuery(ComponentType.ReadWrite<EnemyIdComponent>());
    }

    public void OnUpdate(ref SystemState state)
    {
        var enemiesArray = _queryEnemies.ToComponentDataArray<EnemyIdComponent>(Allocator.Temp);
        var entityStorage = _queryStorage.GetSingletonEntity();
        var statusWave = state.EntityManager.GetComponentData<StorageWaveDataComponent>(entityStorage).StopWave;
        if (statusWave && enemiesArray.Length == 0)
        {
            var simulationSystemGroup = state.World.GetExistingSystemManaged<SimulationSystemGroup>();
            simulationSystemGroup.Enabled = false;
            var statusLevel = new StorageStatusLevelComponent { Victory = true, Stop = true};
            state.EntityManager.SetComponentData(entityStorage, statusLevel);
        }
    }
}
