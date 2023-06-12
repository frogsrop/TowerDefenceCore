using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct MoveEnemySystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;
        new EnemyJob { Dt = dt }.ScheduleParallel();
    }
}

[BurstCompile]
partial struct EnemyJob : IJobEntity
{
    public float Dt;

    public void Execute(
        TransformAspect transform,
        ref DirectionComponent dir,
        ref SpeedComponent speed,
        ref SpeedModifierComponent speedModifier
    ) {
        if ((transform.Position.x > 5 && dir.Direction > 0) ||
            (transform.Position.x < -5 && dir.Direction < 0))
        {
            dir.Direction *= -1;
        }

        float speedCoefficient = 1;
        // TODO: can we overload this function for enemies without SpeedModifierComponent
        // and just disable it?
        if (speedModifier.Timeout > 0) {
            speedCoefficient = speedModifier.ModifierCoefficient;
            speedModifier.Timeout -= Dt;
        }

        transform.Position += new float3(
            Dt * dir.Direction * Mathf.Min(speed.MaxSpeed, speed.Speed * speedCoefficient),
            0, 0
        );
    }
}