using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial class DamageSystem : SystemBase
{
    [BurstCompile]
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);

        Entities.WithAll<DamageComponent>().WithAll<EnemyHPComponent>()
            .ForEach(
                (Entity entity, ref DamageComponent damage, ref EnemyHPComponent hp) =>
                {
                    var hpResult = new EnemyHPComponent { hp = hp.hp - damage.allDamage };
                    var damageReset = new DamageComponent { allDamage = 0 };
                    EntityManager.SetComponentData(entity, hpResult);
                    ecb.SetComponent(entity, damageReset);
                }).WithoutBurst().Run();
        Dependency.Complete();
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
