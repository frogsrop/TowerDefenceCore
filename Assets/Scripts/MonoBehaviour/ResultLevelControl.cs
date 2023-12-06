using Unity.Entities;
using UnityEngine;

public class ResultLevelControl : MonoBehaviour
{
    public GameObject LoseLevel;
    public GameObject VictoryLevel;
    private EntityManager _entityManager;
    private EntityQuery _queryStorage;
    private Entity _entityStorage;
    private int _startLevelHp;
    private int _startWaveLength;
    private int _startCoins;
    
    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _queryStorage = _entityManager.CreateEntityQuery(typeof(StorageStatusLevelComponent));
        _entityStorage = _queryStorage.GetSingletonEntity();
        _startLevelHp = _entityManager.GetComponentData<StorageLevelHpComponent>(_entityStorage).LevelHp;
        _startWaveLength = _entityManager.GetComponentData<StorageWaveDataComponent>(_entityStorage).StartWaveLength;
        _startCoins = _entityManager.GetComponentData<StorageCoinsComponent>(_entityStorage).Coins;
    }

    
    void Update()
    {
        var realHp = _entityManager.GetComponentData<StorageLevelHpComponent>(_entityStorage).LevelHp;
        var status = _entityManager.GetComponentData<StorageStatusLevelComponent>(_entityStorage);
        if (status.Reset) return;
        
        if (status.Lose)
        {
            LoseLevel.SetActive(true);
        }
        if (status.Victory && realHp>0)
        {
            VictoryLevel.SetActive(true);
        }
    }

    public void GoToMenu()
    {
        _entityManager.World.GetExistingSystemManaged<SimulationSystemGroup>().Enabled = true;
        var entityStorage = _queryStorage.GetSingletonEntity();
        var conditionComponent = new StorageLevelHpComponent { LevelHp = _startLevelHp };
        var waveLength = new StorageWaveDataComponent { WaveLength = _startWaveLength, StartWaveLength = _startWaveLength};
        var coins = new StorageCoinsComponent { Coins = _startCoins };
        var statusLevel = new StorageStatusLevelComponent { Reset = true, Stop = true};
        _entityManager.SetComponentData(entityStorage, conditionComponent);
        _entityManager.SetComponentData(entityStorage, waveLength);
        _entityManager.SetComponentData(entityStorage, coins);
        _entityManager.SetComponentData(entityStorage, statusLevel);
    }
}
