using Unity.Entities;
using UnityEngine;

public class StartFirstLevel : MonoBehaviour
{
    private EntityManager _entityManager;
    private EntityQuery _queryStorage;

    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _queryStorage = _entityManager.CreateEntityQuery(typeof(StorageEntitiesComponent));
    }

    public void StartLevel()
    {
        var entityStorage = _queryStorage.GetSingletonEntity();
        var statusStart = new StorageStatusLevelComponent { Start = true };
        _entityManager.SetComponentData(entityStorage, statusStart);
    }
}