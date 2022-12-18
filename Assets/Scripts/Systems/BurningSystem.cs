using System.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public partial class BurningSystem : SystemBase
{
    private Timer _timer = new(1f);

    protected override void OnStartRunning()
    {
        Entities.WithAll<BurningComponent>()
            .ForEach(
                (Entity entity, ref BurningComponent damage) =>
                {
                    EntityManager.SetComponentEnabled<BurningComponent>(entity, false); //TODO: when a creep spawns
                }).WithoutBurst().Run();

    }
    
    [BurstCompile]
    protected override void OnUpdate()
    {
        var dt = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(Allocator.TempJob);

        Entities.WithAll<BurningComponent>().WithAll<EnemyHpComponent>()
            .ForEach(
                (Entity entity, ref BurningComponent damage, ref EnemyHpComponent hp) =>
                {
                    var timerBurning = damage.Timer;

                    if (!_timer.refreshTimerAndCheckFinish())
                    {
                        return;
                    }

                    for (int i= 0; i < timerBurning; i++) 
                    {
                        var timerResult = new BurningComponent { BurningDamage = 2, Timer = timerBurning - 1 };
                        var hpResult = new EnemyHpComponent { Hp = hp.Hp - damage.BurningDamage };
                        EntityManager.SetComponentData(entity, timerResult);
                        EntityManager.SetComponentData(entity, hpResult);
                        return;
                    }
                    EntityManager.SetComponentEnabled<BurningComponent>(entity, false); 
                }).WithoutBurst().Run();
        
        Dependency.Complete();
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }

    
}