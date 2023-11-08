using TMPro;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;
using Unity.Transforms;
using Unity.Mathematics;

public class DropdownMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown _quantityTowersMenu;
    private int valueTowerMenu;
    [SerializeField]    
    private TMP_Dropdown _quantityEnemiesMenu;
    private int valueEnemiesMenu;
    [SerializeField]    
    private TMP_Dropdown _speedShootMenu;
    private int valueSpeedShootMenu;
    
    [SerializeField]
    private float MinSpawnX = -8;
    [SerializeField]
    private float MaxSpawnX = 8;
    [SerializeField]
    private float MinSpawnY = -4;
    [SerializeField]
    private float MaxSpawnY = 2;

    private EntityManager _entityManager;
    private Entity _entityStorage;
    private Entity _entityTower;
    private Entity _entityEnemy;

    private int _quantityTowers;
    private int _quantityEnemies;
    private float _speedShoot;
    
    private Random _random;

    //[SerializeField] private EnemyConfig _enemyConfig;

    public void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public void OnButtonSpawn()
    {
        valueTowerMenu = _quantityTowersMenu.value;
        if (valueTowerMenu == 0) { _quantityTowers = 0; }
        if (valueTowerMenu == 1) { _quantityTowers = 10; }
        if (valueTowerMenu == 2) { _quantityTowers = 100; }
        if (valueTowerMenu == 3) { _quantityTowers = 500; }
        if (valueTowerMenu == 4) { _quantityTowers = 1000; }

        valueEnemiesMenu = _quantityEnemiesMenu.value;
        if (valueEnemiesMenu == 0) { _quantityEnemies = 0; }
        if (valueEnemiesMenu == 1) { _quantityEnemies = 10; }
        if (valueEnemiesMenu == 2) { _quantityEnemies = 100; }
        if (valueEnemiesMenu == 3) { _quantityEnemies = 500; }

        valueSpeedShootMenu = _speedShootMenu.value;
        if (valueSpeedShootMenu == 0) { _speedShoot = 0f; }
        if (valueSpeedShootMenu == 1) { _speedShoot = 0.25f; }
        if (valueSpeedShootMenu == 2) { _speedShoot = 0.5f; }
        if (valueSpeedShootMenu == 3) { _speedShoot = 2f; }
        
        _entityStorage = _entityManager.CreateEntityQuery(typeof(StorageDataComponent))
            .GetSingletonEntity();

        var storageComponent = _entityManager.GetComponentData<StorageDataComponent>(_entityStorage);
        _entityTower = storageComponent.SimpleTowerPrefab;
        _entityEnemy = storageComponent.EnemyPrefab;
        var _minimumPosition = new float3(MinSpawnX, MinSpawnY, 0);
        var _maximumPosition = new float3(MaxSpawnX, MaxSpawnY, 0);
        
        var timeSeed = (uint)(Time.deltaTime * 100000);
        _random.InitState(timeSeed);
        
        for (var i = 1; i <= _quantityTowers; i++)
        {
            var posTowerSpawn = _random.NextFloat3(_minimumPosition, _maximumPosition);
            var towerUniformScaleTransform = new UniformScaleTransform
                { Position = posTowerSpawn, Scale = 0.5f };
            var setSpawnTowerPosition = new LocalToWorldTransform
                { Value = towerUniformScaleTransform };
            var setSpeedAttack = new TowerSpeedAttack { Value = _speedShoot };
            _entityManager.SetComponentData(_entityTower, setSpawnTowerPosition);
            _entityManager.SetComponentData(_entityTower, setSpeedAttack);
            _entityManager.Instantiate(_entityTower);
        }
        for (var i = 1; i <= _quantityEnemies; i++)
        {
            var posEnemySpawn = _random.NextFloat3(_minimumPosition, _maximumPosition);
            var enemyUniformScaleTransform = new UniformScaleTransform
                { Position = posEnemySpawn, Scale = 0.5f };
            _entityManager.SetComponentData(_entityEnemy, new LocalToWorldTransform
                { Value = enemyUniformScaleTransform });
            _entityManager.SetComponentData(_entityEnemy, new EnemyIdComponent { Id =  i});
            _entityManager.Instantiate(_entityEnemy);
        }
    }
}

