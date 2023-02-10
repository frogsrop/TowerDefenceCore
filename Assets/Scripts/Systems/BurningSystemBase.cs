using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial class BurningSystem : SystemBase
{
    private Timer _timer = new(1f);
    
    [BurstCompile]
    protected override void OnCreate()
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var queryBurningComponent = GetEntityQuery(ComponentType.ReadOnly<BurningComponent>());
        new OffBurningComponentJob{ecbJob = ecb}.Run(queryBurningComponent);
    }
    
    [BurstCompile]
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var queryBurningComponent = GetEntityQuery(ComponentType.ReadOnly<BurningComponent>(), 
            ComponentType.ReadOnly<EnemyHpComponent>());
        var dt = SystemAPI.Time.DeltaTime;
        if (!_timer.refreshTimerAndCheckFinish(dt))
        {
            return;
        }
        new BurningJob{ecbJob=ecb}.Run(queryBurningComponent);
        Dependency.Complete();
        ecb.Playback(EntityManager);
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

[BurstCompile]
public partial struct BurningJob : IJobEntity
{
    public EntityCommandBuffer ecbJob;
    private void Execute(Entity entity, ref BurningComponent damage, ref EnemyHpComponent hp)
    {
        var timerBurning = damage.Timer;

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