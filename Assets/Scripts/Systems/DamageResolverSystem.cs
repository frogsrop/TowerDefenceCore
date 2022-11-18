using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial class DamageResolverSystem : SystemBase
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
                res += en.Current.damage;
            }

            return res;
        }

        Entities.WithAll<DamageBufferElement>()
            .ForEach(
                (Entity entity,ref DynamicBuffer<DamageBufferElement> buffer) =>
                {
                    if (buffer.Length > 0)
                    {
                        EntityManager.HasComponent<DamageComponent>(entity);
                        var res = lfold(buffer.GetEnumerator());
                        buffer.Clear();
                        var allDamage = new DamageComponent { allDamage = res };
                        ecb.SetComponent(entity, allDamage);
                    }
                }).WithoutBurst().Run();

        Dependency.Complete();
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}