using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    //[SerializeField] private SubScene _subScene;
    private EntityManager _entityManager;
    private EntityQuery _queryStorage;
    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _queryStorage = _entityManager.CreateEntityQuery(
            typeof(StorageEditSceneComponent));
    }
    
    public void Restart()
    {
        var entityStorage = _queryStorage.GetSingletonEntity();
        var restarComponent = new StorageEditSceneComponent { Reset = true };
        _entityManager.SetComponentData(entityStorage, restarComponent);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }
}
