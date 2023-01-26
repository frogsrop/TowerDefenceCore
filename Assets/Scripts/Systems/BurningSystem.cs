using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
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

                    if (timerBurning >= 0) 
                    {
                        var timerResult = new BurningComponent { BurningDamage = damage.BurningDamage, 
                            Timer = timerBurning - 1 };
                        var resHp = hp.Hp - damage.BurningDamage;
                        var hpResult = new EnemyHpComponent { Hp =  resHp, MaxHp = hp.MaxHp};
                        
                        var children = EntityManager.GetBuffer<Child>(entity);
                        foreach (var child in children)
                        {
                            var ob = EntityManager.GetComponentObject<SpriteRenderer>(child.Value);
                            Debug.Log(ob.name);
                            if (ob.name == "hp(Clone)(Clone)(Clone)")
                            {
                                ob.size = new Vector2(Mathf.Max(0f, resHp * 1f) / hp.MaxHp, 1);
                                break;
                            }
                        }
                        
                        EntityManager.SetComponentData(entity, timerResult);
                        EntityManager.SetComponentData(entity, hpResult);
                    }
                    else
                    {
                        EntityManager.SetComponentEnabled<BurningComponent>(entity, false);
                    }
                     
                }).WithoutBurst().Run();
        Dependency.Complete();
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}