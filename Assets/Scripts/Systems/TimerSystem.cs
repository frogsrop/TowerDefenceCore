using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct TimerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // var ecb = new EntityCommandBuffer(Allocator.TempJob);
        // var queryTimerComponent = state.GetEntityQuery(ComponentType.ReadOnly<TimerComponent>());
        // new OffTimerComponentJob{ecbJob = ecb}.Run(queryTimerComponent);
    }
    [BurstCompile] public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var dt = SystemAPI.Time.DeltaTime;
        var queryTimerComponent = state.GetEntityQuery(ComponentType.ReadOnly<TimerComponent>(), ComponentType.ReadOnly<BurningBufferElement>());
        new TimerJob{DtJob = dt, EcbJob = ecb}.Run(queryTimerComponent);
    }
}

[BurstCompile]
public partial struct TimerJob : IJobEntity
{
    public float DtJob;
    public EntityCommandBuffer EcbJob;
    private void Execute(Entity entity, ref TimerComponent timer)
    {
        if(!timer.Condition) return;
        var timeout = timer.Time - DtJob;
        UnityEngine.Debug.Log("timerJobExecute");
        if (timeout <= 0)
        {
            //SetTime(1f, true, entity);
            var updateTime = new TimerComponent {Time = 1f, Trigger = true, Condition = true};
            UnityEngine.Debug.Log("obnovil");
            EcbJob.SetComponent(entity, updateTime);
        }
        else
        {
            //SetTime(timeout, true, entity);
            UnityEngine.Debug.Log("raznica");
            var updateTime = new TimerComponent {Time = timeout, Trigger = true, Condition = true};
            EcbJob.SetComponent(entity, updateTime);
        }
        
    }

    void SetTime(float time, bool result,Entity entity)
    {
        UnityEngine.Debug.Log("voidSetTime");
        var updateTime = new TimerComponent {Time = time, Trigger = result, Condition = true};
        EcbJob.SetComponent(entity, updateTime);
    }
}

// [BurstCompile]
// public partial struct OffTimerComponentJob : IJobEntity
// {
//     public EntityCommandBuffer ecbJob;
//     private void Execute(Entity entity, ref TimerComponent timer)
//     { 
//         ecbJob.SetComponentEnabled<TimerComponent>(entity, false);/*TODO: when a creep spawns*/
//     }
// }