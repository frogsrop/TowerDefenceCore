using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct EffectResolverSystem : ISystem
{
    private EntityQuery _queryBurningBuffer; 
    private EntityQuery _queryDamageBuffer;

    [BurstCompile] 
    public void OnCreate(ref SystemState state) 
    {
        _queryDamageBuffer = state.GetEntityQuery(ComponentType.ReadOnly<DamageBufferElement>());
        _queryBurningBuffer = state.GetEntityQuery(ComponentType.ReadOnly<BurningBufferElement>());
    }
    [BurstCompile] public void OnDestroy(ref SystemState state) {}
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        new DamageResolverJob { Ecb = ecb }.Run(_queryDamageBuffer);
        new BurnResolverJob { Ecb = ecb }.Run(_queryBurningBuffer);
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

public partial struct DamageResolverJob : IJobEntity
{
    public EntityCommandBuffer Ecb;

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
            Ecb.SetComponentEnabled<DamageComponent>(entity, true);
            var res = lfold(damageBuffer.GetEnumerator());
            damageBuffer.Clear();
            var damage = new DamageComponent { Damage = res };
            Ecb.SetComponent(entity, damage);
        }
    }
}

public partial struct BurnResolverJob : IJobEntity
{
    public EntityCommandBuffer Ecb;

    private void Execute(Entity entity, ref DynamicBuffer<BurningBufferElement> burningBuffer, 
        ref TimerComponent timerComponent)
    {
        var mapping = AbstractEffectConfig.Mapping;
        if (burningBuffer.Length > 0)
        {
            Ecb.SetComponentEnabled<BurningComponent>(entity, true);
            if(!timerComponent.Condition) Ecb.SetComponent(entity, 
                new TimerComponent{Condition = true, Trigger = false, Time = 1f, Delay = 1f});
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
            Ecb.SetComponent(entity,
                new BurningComponent
                    { BurningDamage = maxBurningEffect.Damage, Timer = maxBurningEffect.Timer });
            burningBuffer.Clear();
        }
    }
}