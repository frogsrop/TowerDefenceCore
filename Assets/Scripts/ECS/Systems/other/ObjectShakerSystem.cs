using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class ObjectShakerSystem : SystemBase
{
    [BurstCompile]
    protected override void OnUpdate()
    {
        var dt = Time.DeltaTime;
        Entities.WithAll<SpriteRenderer>()
            .ForEach((ref Translation translation, ref DirectionComponent dir) =>
            {
                if ((translation.Value.x > 2 && dir.Direction > 0) ||
                    (translation.Value.x < -2 && dir.Direction < 0))
                {
                    dir.Direction *= -1;
                }

                translation.Value += new float3(dt * dir.Direction, 0, 0);
            }).WithBurst().ScheduleParallel();
    }
}