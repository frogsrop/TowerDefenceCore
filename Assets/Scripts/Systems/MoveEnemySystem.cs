using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct EnemyJob : IJobEntity
{
    public float dt;
    public void Execute(TransformAspect transform, ref DirectionComponent dir)
    {
        if ((transform.Position.x > 5 && dir.Direction > 0) ||
            (transform.Position.x < -5 && dir.Direction < 0))
        {
            dir.Direction *= -1;
        }

        transform.Position += new float3(dt * dir.Direction, 0, 0);
    }
}

partial class MoveEnemySystem : SystemBase
{
    [BurstCompile]
    protected override void OnUpdate()
    {
        var dt = SystemAPI.Time.DeltaTime;
        UnityEngine.Debug.Log(dt);
        new EnemyJob { dt = dt }.ScheduleParallel();
    }
}