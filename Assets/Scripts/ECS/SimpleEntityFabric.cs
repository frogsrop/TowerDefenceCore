using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS
{
    [BurstCompile]
    public partial class MovingSystem : SystemBase
    {
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
                }).Run();
        }
    }
}