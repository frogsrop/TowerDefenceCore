// using Unity.Burst;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Transforms;
// using UnityEngine;
//
// [BurstCompile]
// public partial class BurningISystem : SystemBase
// {
//     [BurstCompile]
//     protected override void OnStartRunning()
//     {
//         var ecb = new EntityCommandBuffer(Allocator.TempJob);
//         var queryBurningComponent = GetEntityQuery(ComponentType.ReadOnly<BurningComponent>());
//         new OffBurningComponentJob{ecbJob = ecb}.Run(queryBurningComponent);
//     }
//     
//     [BurstCompile]
//     protected override void OnUpdate()
//     {
//         var ecb = new EntityCommandBuffer(Allocator.TempJob);
//         var queryBurningComponent = GetEntityQuery(ComponentType.ReadOnly<BurningComponent>(), 
//             ComponentType.ReadOnly<EnemyHpComponent>());
//         new BurningJob().Run(queryBurningComponent);
//         Dependency.Complete();
//         ecb.Playback(EntityManager);
//         ecb.Dispose();
//     }
// }
//
// [BurstCompile]
// public partial struct OffBurningComponentJob : IJobEntity
// {
//     public EntityCommandBuffer ecbJob;
//     private void Execute(Entity entity, ref BurningComponent damage)
//     {
//         ecbJob.SetComponentEnabled<BurningComponent>(entity, false); /*TODO: when a creep spawns*/
//     }
// }
//
// [BurstCompile]
// public partial struct BurningJob : IJobEntity
// {
//     private void Execute(Entity entity, ref BurningComponent damage, ref EnemyHpComponent hp)
//     {
//         var ecb = new EntityCommandBuffer();
//         var timer = new Timer(1f);
//         var timerBurning = damage.Timer;
//
//         if (!timer.refreshTimerAndCheckFinish()) return;
//         
//         if (timerBurning >= 0) 
//         {
//             var timerResult = new BurningComponent { BurningDamage = damage.BurningDamage, Timer = timerBurning - 1 };
//             var resHp = hp.Hp - damage.BurningDamage;
//             var hpResult = new EnemyHpComponent { Hp =  resHp, MaxHp = hp.MaxHp};
//             var children = ecb.GetBuffer<Child>(entity);
//             foreach (var child in children)
//             {
//                 var ob = ecb.GetComponentObject<SpriteRenderer>(child.Value);
//                 Debug.Log(ob.name);
//                 if (ob.name == "hp(Clone)(Clone)(Clone)")
//                 {
//                     ob.size = new Vector2(Mathf.Max(0f, resHp * 1f) / hp.MaxHp, 1);
//                     break;
//                 }
//             }
//             ecb.SetComponentData(entity, timerResult);
//             ecb.SetComponentData(entity, hpResult);
//         }
//         else
//         {
//             ecb.SetComponentEnabled<BurningComponent>(entity, false);
//         }
//     }
//     
// }