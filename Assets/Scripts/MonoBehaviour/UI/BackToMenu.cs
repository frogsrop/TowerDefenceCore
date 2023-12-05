// using Unity.Entities;
// using UnityEngine;
//
// public class BackToMenu : MonoBehaviour
// {
//     private EntityManager _entityManager;
//     private EntityQuery _queryStorage;
//     
//     void Start()
//     {
//         _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
//         _queryStorage = _entityManager.CreateEntityQuery(typeof(StorageEntitiesComponent));
//     }
//
//     public void ToMenu()
//     {
//         _entityManager.World.GetExistingSystemManaged<SimulationSystemGroup>().Enabled = true;
//         
//     }
// }