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

    private EntityManager _entityManager;
    private Entity _entityStorage;
    private Entity _entityTower;
    private Entity _entityEnemy;

    private int _quantityTowers;
    private int _quantityEnemies;
    private float _speedShoot;
    
    private Random _random;

    [SerializeField] private EnemyConfig _enemyConfig;
    // [SerializeField]
    // private int EnemyMaxHp = 500;
    // [SerializeField]
    // private int EnemyDirection = 5;
    // [SerializeField]
    // private int EnemySpeed = 4;
    // private int enemyId =0;
    // [SerializeField]
    // private GameObject AnimPrefab;

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

        
        _entityStorage = _entityManager.CreateEntityQuery(typeof(StoragePrefabsComponent))
            .GetSingletonEntity();

        var storageComponent = _entityManager.GetComponentData<StoragePrefabsComponent>(_entityStorage);
        _entityTower = storageComponent.TowerPrefab;
        //_entityEnemy = storageComponent.EnemyPrefab;
        var _minimumPosition = new float3(-8, -4, 0);
        var _maximumPosition = new float3(8, 2, 0);
        
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
            var EnemyPrefab = _entityManager.CreateEntity();
            _entityManager.AddComponentData(EnemyPrefab, new LocalToWorldTransform
                { Value = enemyUniformScaleTransform });
            _entityManager.AddComponentData(EnemyPrefab, new EnemyHpComponent
            {
                Hp = _enemyConfig.EnemyMaxHp, MaxHp = _enemyConfig.EnemyMaxHp
            });
            _entityManager.AddComponentData(EnemyPrefab, new DirectionComponent
            {
                Direction = _enemyConfig.EnemyDirection
            });
            _entityManager.AddComponentData(EnemyPrefab, new EnemyIdComponent { Id = i });
            _entityManager.AddComponent<TimerComponent>(EnemyPrefab);
            _entityManager.AddComponent<DamageComponent>(EnemyPrefab);
            _entityManager.AddComponent<BurningComponent>(EnemyPrefab);
            _entityManager.AddBuffer<DamageBufferElement>(EnemyPrefab);
            _entityManager.AddBuffer<BurningBufferElement>(EnemyPrefab);
            
            if (_enemyConfig.EnemySpeed > 0)
            {
                SpeedComponent speed = default;
                speed.Value = _enemyConfig.EnemySpeed;
                _entityManager.AddComponentData(EnemyPrefab, speed);
            }

            PresentationGoComponent pgo = new PresentationGoComponent();
            pgo.Prefab = _enemyConfig.AnimPrefab;
            _entityManager.AddComponentObject(EnemyPrefab, pgo);
        }
    }
    
    // public void CreateEnemy(int id)
    // {
    //     var EnemyPrefab = _entityManager.CreateEntity();
    //
    //     _entityManager.AddComponentData(EnemyPrefab, new EnemyHpComponent { Hp = EnemyMaxHp, MaxHp = EnemyMaxHp });
    //     _entityManager.AddComponentData(EnemyPrefab, new DirectionComponent { Direction = EnemyDirection });
    //     _entityManager.AddComponentData(EnemyPrefab, new EnemyIdComponent { Id = enemyId });
    //     _entityManager.AddComponent<TimerComponent>(EnemyPrefab);
    //
    //     var te = new EnemyIdComponent { Id = enemyId };
    //     _entityManager.AddComponent<EnemyIdComponent>(EnemyPrefab);
    //     _entityManager.AddComponent<DamageComponent>(EnemyPrefab);
    //     _entityManager.AddComponent<BurningComponent>(EnemyPrefab);
    //     _entityManager.AddBuffer<DamageBufferElement>(EnemyPrefab);
    //     _entityManager.AddBuffer<BurningBufferElement>(EnemyPrefab);
    //     if (EnemySpeed > 0)
    //     {
    //         SpeedComponent speed = default;
    //         speed.Value = EnemySpeed;
    //         _entityManager.AddComponentData(EnemyPrefab, speed);
    //     }
    //
    //     PresentationGoComponent pgo = new PresentationGoComponent();
    //     pgo.Prefab = AnimPrefab;
    //     _entityManager.AddComponentObject(EnemyPrefab, pgo);
    //
    //
    //     _entityEnemy = EnemyPrefab;
    // }
}