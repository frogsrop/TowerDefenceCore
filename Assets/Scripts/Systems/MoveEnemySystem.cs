using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct MoveEnemySystem : ISystem
{
    [BurstCompile] public void OnCreate(ref SystemState state) {}
    [BurstCompile] public void OnDestroy(ref SystemState state) {}
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;
        new EnemyJob{Dt = dt}.ScheduleParallel();
    }
}

[BurstCompile]
partial struct EnemyJob : IJobEntity
{
    public float Dt;
    public void Execute(TransformAspect transform, ref DirectionComponent dir)
    {
        if ((transform.Position.x > 5 && dir.Direction > 0) ||
            (transform.Position.x < -5 && dir.Direction < 0))
        {
            dir.Direction *= -1;
        }

        transform.Position += new float3(Dt * dir.Direction, 0, 0);
    }
}

