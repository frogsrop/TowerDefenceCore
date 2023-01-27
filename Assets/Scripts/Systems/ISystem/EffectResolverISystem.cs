using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct EffectResolverSystem : ISystem
{
    [BurstCompile] public void OnCreate(ref SystemState state) {}
    [BurstCompile] public void OnDestroy(ref SystemState state) {}
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        ApplyDamage(ecb, ref state);
        ApplyBurn(ecb, ref state);
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [BurstCompile]
    void ApplyDamage(EntityCommandBuffer ecb, ref SystemState state)
    {
        var queryDamageBuffer = state.GetEntityQuery(ComponentType.ReadOnly<DamageBufferElement>());
        new DamageJob { ecbJob = ecb }.Run(queryDamageBuffer);
    }
    
    [BurstCompile]
    void ApplyBurn(EntityCommandBuffer ecb, ref SystemState state)
    {
        var queryBurningBuffer = state.GetEntityQuery(ComponentType.ReadOnly<BurningBufferElement>());
        new BurnJob { ecbJob = ecb }.Run(queryBurningBuffer);
    }
}

public partial struct DamageJob : IJobEntity
{
    public EntityCommandBuffer ecbJob;

    private void Execute(Entity entity, ref DynamicBuffer<DamageBufferElement> damageBuffer)
    {
        var mapping = AbstractEffectConfig.Mapping;
        int lfold(NativeArray<DamageBufferElement>.Enumerator en)
        {
            int res = 0;
            while (en.MoveNext())
            {
                var damage = (DamageEffectConfig)mapping[en.Current.Id];
                res += damage.Damage;
            }
            return res;
        }
        if (damageBuffer.Length > 0)
        {
            ecbJob.SetComponentEnabled<DamageComponent>(entity, true);
            var res = lfold(damageBuffer.GetEnumerator());
            damageBuffer.Clear();
            var damage = new DamageComponent { Damage = res };
            ecbJob.SetComponent(entity, damage);
        }
    }
}

[BurstCompile]
public partial struct BurnJob : IJobEntity
{
    public EntityCommandBuffer ecbJob;
    private void Execute(Entity entity, ref DynamicBuffer<BurningBufferElement> burningBuffer)
    {
        var mapping = AbstractEffectConfig.Mapping;
        if (burningBuffer.Length > 0)
        {
            ecbJob.SetComponentEnabled<BurningComponent>(entity, true);
            var resId = 0;
            var timer = -1f;
            foreach (var burning in burningBuffer)
            {
                var burningEffectConfig = (BurningEffectConfig)mapping[burning.Id];
                if (burningEffectConfig.Timer > timer)
                {
                    timer = burningEffectConfig.Timer;
                    resId = burning.Id;
                }
            }
            var maxBurningEffect = (BurningEffectConfig)mapping[resId];
            ecbJob.SetComponent(entity,
                new BurningComponent
                    { BurningDamage = maxBurningEffect.Damage, Timer = maxBurningEffect.Timer });
            burningBuffer.Clear();
        }
    }
}