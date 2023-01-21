using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial class DamageSystem : SystemBase
{
    protected override void OnStartRunning()
    {
        Entities.WithAll<DamageComponent>()
            .ForEach(
                (Entity entity, ref DamageComponent damage) =>
                {
                    EntityManager.SetComponentEnabled<DamageComponent>(entity, false); //TODO: when a creep spawns
                }).WithoutBurst().Run();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);

        Entities.WithAll<DamageComponent>().WithAll<EnemyHpComponent>()
            .ForEach(
                (Entity entity, ref DamageComponent damage, ref EnemyHpComponent hp) =>
                {
                    var resHp = hp.Hp - damage.Damage;
                    var hpResult = new EnemyHpComponent { Hp = resHp , MaxHp = hp.MaxHp};
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
                    EntityManager.SetComponentData(entity, hpResult);
                    EntityManager.SetComponentEnabled<DamageComponent>(entity, false);
                }).WithoutBurst().Run();
        Dependency.Complete();
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}