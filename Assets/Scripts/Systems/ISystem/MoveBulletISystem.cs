// using System.Collections.Generic;
// using Unity.Burst;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Mathematics;
// using Unity.Transforms;
//
// [BurstCompile]
// public partial struct MoveBulletISystem : ISystem
// {
//     
//     [BurstCompile]
//     public void OnCreate(ref SystemState state)
//     {
//         throw new System.NotImplementedException();
//     }
//      
//      [BurstCompile]
//      public void OnDestroy(ref SystemState state)
//      {
//          throw new System.NotImplementedException();
//      }
//
//      [BurstCompile]
//      public void OnUpdate(ref SystemState state)
//      {
//          var targetIdQuery = state.GetEntityQuery(ComponentType.ReadOnly<TargetIdComponent>());
//          var queryEnemy = state.GetEntityQuery(ComponentType.ReadOnly<EnemyIdComponent>(),
//              ComponentType.ReadOnly<LocalToWorldTransform>(),
//              ComponentType.ReadOnly<DamageBufferElement>(),
//              ComponentType.ReadOnly<BurningBufferElement>());
//          var enemyIds = queryEnemy.ToComponentDataArray<EnemyIdComponent>(Allocator.TempJob);
//          var ecb = new EntityCommandBuffer(Allocator.TempJob);
//          if (enemyIds.Length > 0)
//          {
//              var dt = SystemAPI.Time.DeltaTime;
//              new MoveBulletJob
//              {
//                  dtJob = dt,
//                  ecbJob = ecb,
//                  enemyIdsJob = enemyIds,
//                  enemyTransformsJob = queryEnemy.ToComponentDataArray<LocalToWorldTransform>(Allocator.TempJob),
//                  enemyEntityArrayJob = queryEnemy.ToEntityArray(Allocator.TempJob)
//              }.Run(targetIdQuery);
//              state.Dependency.Complete();
//              ecb.Playback(state.EntityManager);
//              ecb.Dispose();
//          }
//      }
// }
//
// [BurstCompile]
// public partial struct MoveBulletJob : IJobEntity
// {
//     public float dtJob;
//     public EntityCommandBuffer ecbJob;
//     public NativeArray<EnemyIdComponent> enemyIdsJob;
//     public NativeArray<LocalToWorldTransform> enemyTransformsJob;
//     public NativeArray<Entity> enemyEntityArrayJob;
//
//     private static Dictionary<int, AbstractEffectConfig> _mapping;
//
//     public void Execute(ref LocalToWorldTransform bulletTransform, in TargetIdComponent bullet, in BulletComponent bulletInfo,
//         in Entity entity)
//     {
//         _mapping = AbstractEffectConfig.Mapping;
//         var enemyIndex = IndexOf(enemyIdsJob, bullet.Id);
//         if (enemyIndex != -1)
//         {
//             var enemyTransform = enemyTransformsJob[enemyIndex];
//             var enemyEntity = enemyEntityArrayJob[enemyIndex];
//             var direction = math.normalize(enemyTransform.Value.Position - bulletTransform.Value.Position);
//             bulletTransform.Value.Position += direction * dtJob * 10;
//             var distance = math.distancesq(bulletTransform.Value.Position, enemyTransform.Value.Position);
//             if (distance < 0.1f)
//             {
//                 ecbJob.DestroyEntity(entity);
                 // foreach (var effect in bulletInfo.ListEffects)
                 // {
                 //     if (_mapping.ContainsKey(effect))
                 //     {
                 //         _mapping[effect].AppendToBuffer(enemyEntity, ecbJob);
                 //     }
//                 }
//             }
//         }
//         else
//         {
//             ecbJob.DestroyEntity(entity);
//         }
//     }
//     
//     int IndexOf(NativeArray<EnemyIdComponent> array, int id)
//     {
//         for (int i = 0; i < array.Length; i++)
//         {
//             if (array[i].Id == id)
//             {
//                 return i;
//             }
//         }
//
//         return -1;
//     }
// }