using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial class EffectResolverSystem : SystemBase
{
    [BurstCompile]
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);

        int lfold(NativeArray<DamageBufferElement>.Enumerator en)
        {
            int res = 0;
            while (en.MoveNext())
            {
                res += en.Current.Damage;
            }

            return res;
        }

        Entities.WithAll<DamageBufferElement>()
            .ForEach(
                (Entity entity, ref DynamicBuffer<DamageBufferElement> damageBuffer) =>
                {
                    if (damageBuffer.Length > 0)
                    {
                        EntityManager.SetComponentEnabled<DamageComponent>(entity, true);
                        var res = lfold(damageBuffer.GetEnumerator());
                        damageBuffer.Clear();
                        var damage = new DamageComponent { Damage = res };
                        ecb.SetComponent(entity, damage);
                    }
                }).WithoutBurst().Run();

        Entities.WithAll<BurningBufferElement>()
            .ForEach(
                (Entity entity, ref DynamicBuffer<BurningBufferElement> burningBuffer) =>
                {
                    
                    if (burningBuffer.Length > 0)
                    {
                        EntityManager.SetComponentEnabled<BurningComponent>(entity, true);
                        burningBuffer.Clear();
                        var burn = new BurningComponent { BurningDamage = 2, Timer = 4 };
                        ecb.SetComponent(entity, burn);
                    }
                }).WithoutBurst().Run();
        Dependency.Complete();
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}