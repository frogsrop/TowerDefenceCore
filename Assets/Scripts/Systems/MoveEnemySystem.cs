using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

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

partial class MoveEnemySystem : SystemBase
{
    [BurstCompile]
    protected override void OnUpdate()
    {
        var dt = SystemAPI.Time.DeltaTime;
        new EnemyJob { Dt = dt }.ScheduleParallel();
    }
}