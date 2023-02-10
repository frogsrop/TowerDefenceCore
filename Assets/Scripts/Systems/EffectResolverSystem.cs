// using System.Collections.Generic;
// using System.Linq;
// using Unity.Burst;
// using Unity.Collections;
// using Unity.Entities;
//
// [BurstCompile]
// public partial class EffectResolverSystem : SystemBase
// {
//     private static Dictionary<int, AbstractEffectConfig> mapping;
//
//     protected override void OnStartRunning()
//     {
//         mapping = AbstractEffectConfig.Mapping;
//     }
//
//     [BurstCompile]
//     protected override void OnUpdate()
//     {
//         var ecb = new EntityCommandBuffer(Allocator.TempJob);
//
//         ApplyDamage(ecb);
//         ApplyBurn(ecb);
//
//         Dependency.Complete();
//         ecb.Playback(EntityManager);
//         ecb.Dispose();
//     }
//
//     void ApplyDamage(EntityCommandBuffer ecb)
//     {
//         int lfold(NativeArray<DamageBufferElement>.Enumerator en)
//         {
//             int res = 0;
//             while (en.MoveNext())
//             {
//                 var damage = (DamageEffectConfig)mapping[en.Current.Id];
//
//                 res += damage.Damage;
//             }
//
//             return res;
//         }
//
//         Entities.WithAll<DamageBufferElement>()
//             .ForEach(
//                 (Entity entity, ref DynamicBuffer<DamageBufferElement> damageBuffer) =>
//                 {
//                     if (damageBuffer.Length > 0)
//                     {
//                         EntityManager.SetComponentEnabled<DamageComponent>(entity, true);
//                         var res = lfold(damageBuffer.GetEnumerator());
//                         damageBuffer.Clear();
//                         var damage = new DamageComponent { Damage = res };
//                         ecb.SetComponent(entity, damage);
//                     }
//                 }).WithoutBurst().Run();
//     }
//
//     void ApplyBurn(EntityCommandBuffer ecb)
//     {
//         Entities.WithAll<BurningBufferElement>()
//             .ForEach(
//                 (Entity entity, ref DynamicBuffer<BurningBufferElement> burningBuffer) =>
//                 {
//                     if (burningBuffer.Length > 0)
//                     {
//                         EntityManager.SetComponentEnabled<BurningComponent>(entity, true);
//                         var resId = 0;
//                         var timer = -1f;
//                         foreach (var burning in burningBuffer)
//                         {
//                             var burningEffectConfig = (BurningEffectConfig)mapping[burning.Id];
//                             if (burningEffectConfig.Timer > timer)
//                             {
//                                 timer = burningEffectConfig.Timer;
//                                 resId = burning.Id;
//                             }
//                         }
//
//                         var maxBurningEffect = (BurningEffectConfig)mapping[resId];
//                         ecb.SetComponent(entity,
//                             new BurningComponent
//                                 { BurningDamage = maxBurningEffect.Damage, Timer = maxBurningEffect.Timer });
//
//                         burningBuffer.Clear();
//                     }
//                 }).WithoutBurst().Run();
//     }
// }