using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct BurningISystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var queryBurningComponent = state.GetEntityQuery(ComponentType.ReadOnly<BurningComponent>());
        new OffBurningComponentJob{ecbJob = ecb}.Run(queryBurningComponent);
    }
    [BurstCompile] public void OnDestroy(ref SystemState state) {}
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var queryBurningComponent = state.GetEntityQuery(ComponentType.ReadOnly<BurningComponent>(), 
            ComponentType.ReadOnly<EnemyHpComponent>());
        var dt = SystemAPI.Time.DeltaTime;
        new BurningJob{ecbJob=ecb, dt = dt}.Run(queryBurningComponent);
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

[BurstCompile]
public partial struct OffBurningComponentJob : IJobEntity
{
    public EntityCommandBuffer ecbJob;
    private void Execute(Entity entity, ref BurningComponent damage)
    {
        ecbJob.SetComponentEnabled<BurningComponent>(entity, false); /*TODO: when a creep spawns*/
    }
}

//[BurstCompile]
public partial struct BurningJob : IJobEntity
{
    public float dt;
    public EntityCommandBuffer ecbJob;
    private void Execute(Entity entity, ref BurningComponent damage, ref EnemyHpComponent hp)
    {
        var timer = new Timer(1f);
        var timerBurning = damage.Timer;
        

        if (!timer.refreshTimerAndCheckFinish(dt))
        {
            return;
        }
        
        if (timerBurning >= 0) 
        {
            var timerResult = new BurningComponent { BurningDamage = damage.BurningDamage, Timer = timerBurning - 1 };
            var resHp = hp.Hp - damage.BurningDamage;
            var hpResult = new EnemyHpComponent { Hp =  resHp, MaxHp = hp.MaxHp};
            ecbJob.SetComponent(entity, timerResult);
            ecbJob.SetComponent(entity, hpResult);
        }
        else
        {
            ecbJob.SetComponentEnabled<BurningComponent>(entity, false);
        }
    }
}