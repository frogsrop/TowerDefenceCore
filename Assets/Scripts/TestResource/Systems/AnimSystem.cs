// using Unity.Collections;
// using UnityEngine;
// using Unity.Entities;
// using Unity.Transforms;
// using UnityEngine.SocialPlatforms;
//
// public partial class AnimSystem : SystemBase
// {
//     public Sprite Sprite;
//     
//     public void Init(Sprite particleSystem)
//     {
//         this.Sprite = particleSystem;
//         particleSystemTransform = particleSystem.transform;
//         Enabled = true; // Everything is ready, can begin running the system
//     }
//     public void OnCreate(ref SystemState state)
//     {
//         base.OnCreate();
//         Enabled = false;
//     }
//
//     protected override void OnUpdate()
//     {
//         Entities.WithAll<VfxEmitter>().WithAll<LocalToWorldTransform>().ForEach((in LocalToWorldTransform translation) =>
//         {
//             sparticleSystemTransform.position = translation.Value.Position;
//             sparticleSystem.Emit(1);
//         }).WithoutBurst().Run();
//     }
// }