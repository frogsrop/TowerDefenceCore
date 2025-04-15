using TMPro;
using UnityEngine;
using Unity.Entities;

public class ShowCoins : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;

    private EntityManager _entityManager;
    private Entity _entityStorage;


    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _entityStorage = _entityManager.CreateEntityQuery(
            typeof(StorageDataComponent)).GetSingletonEntity();
    }

    void Update()
    {
        _coinsText.text = _entityManager.GetComponentData<StorageDataComponent>(_entityStorage).Coins.ToString();
    }
}