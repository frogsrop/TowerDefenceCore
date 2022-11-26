using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

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
                    var hpResult = new EnemyHpComponent { Hp = hp.Hp - damage.Damage };
                    EntityManager.SetComponentData(entity, hpResult);
                    EntityManager.SetComponentEnabled<DamageComponent>(entity, false);
                }).WithoutBurst().Run();
        Dependency.Complete();
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
