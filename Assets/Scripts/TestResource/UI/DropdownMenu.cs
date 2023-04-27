using TMPro;
using Unity.Entities;
using UnityEngine;

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
    private Entity _entity;

    private int _quantityTowers;
    private int _quantityEnemies;
    private float _speedShoot;

    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public void OnButtonSpawn()
    {
        _entity = _entityManager.CreateEntityQuery(typeof(TestComponent)).GetSingletonEntity();
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

        var QuantitySpawnComponent = new QuantitySpawnComponent
        {
            QuantityEnemies = _quantityEnemies,
            QuantityTowers = _quantityTowers,
            SpeedShoot = _speedShoot,
            OnOff = true
        };
        _entityManager.SetComponentData(_entity, QuantitySpawnComponent);
    }
}