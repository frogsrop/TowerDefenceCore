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
                (Entity entity, ref DynamicBuffer<DamageBufferElement> buffer) =>
                {
                    if (buffer.Length > 0)
                    {
                        EntityManager.SetComponentEnabled<DamageComponent>(entity, true);
                        var res = lfold(buffer.GetEnumerator());
                        buffer.Clear();
                        var damage = new DamageComponent { Damage = res };
                        ecb.SetComponent(entity, damage);
                    }
                }).WithoutBurst().Run();

        Dependency.Complete();
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}