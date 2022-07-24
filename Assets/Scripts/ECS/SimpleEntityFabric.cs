using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Jobs;
namespace ECS
{
    public partial class MovingSystem : SystemBase
    {
        
        [BurstCompile]
        protected override void OnUpdate()
        {
            var dt = Time.DeltaTime;
            Entities.WithAll<SpriteRenderer>()
                .ForEach((ref Translation translation, ref DirectionComponent dir) =>
                {
                    if ((translation.Value.x > 5 && dir.direction > 0) ||
                        (translation.Value.x < -5 && dir.direction < 0))
                    {
                        dir.direction *= -1;
                    }
            
                    translation.Value += new float3(dt * dir.direction, 0, 0);
                }).WithBurst().ScheduleParallel();
        }
    }
}