// using Unity.Burst;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Scenes;
//
// public partial struct RestartSceneSystem : ISystem
// {
//     private EntityQuery _queryStorage;
//     private EntityQuery newRequest;
//
//     public void OnCreate(ref SystemState state)
//     {
//         _queryStorage = state.GetEntityQuery(
//             ComponentType.ReadWrite<StorageEditSceneComponent>());
//         newRequest = state.GetEntityQuery(typeof(StorageSceneComponent));
//     }
//
//     public void OnUpdate(ref SystemState state)
//     {
//         var entityStorage = _queryStorage.GetSingletonEntity();
//         var levelCondition = state.EntityManager.GetComponentData<StorageEditSceneComponent>(entityStorage).Reset;
//         var referenceScene = state.EntityManager.GetComponentData<StorageSceneComponent>(entityStorage).SceneReference;
//         if (levelCondition)
//         {
//
//         }
//     }
// }