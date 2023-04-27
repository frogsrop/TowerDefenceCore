using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public partial struct DamageSystem : ISystem
{
    private EntityQuery _queryDamageComponent;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        //Debug.Log("DamageSystem - OnCreate");
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        _queryDamageComponent = state.GetEntityQuery(ComponentType.ReadWrite<DamageComponent>());
        new OffDamageComponentJob{Ecb = ecb}.Run(_queryDamageComponent);
    }
    
    [BurstCompile] public void OnDestroy(ref SystemState state) {}

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //Debug.Log("DamageSystem - OnUpdate");
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        new DamageJob{Ecb = ecb}.Run(_queryDamageComponent);
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
    
    [BurstCompile]
    public partial struct OffDamageComponentJob : IJobEntity
    {
        public EntityCommandBuffer Ecb;

        [BurstCompile]
        private void Execute(Entity entity, ref DamageComponent damage)
        {
            //Debug.Log("DamageSystem - OffDamageComponentJob");
            Ecb.SetComponentEnabled<DamageComponent>(entity, false); /*TODO: when a creep spawns*/
        }
    }
    
    [BurstCompile]
    public partial struct DamageJob : IJobEntity
    {
        public EntityCommandBuffer Ecb;

        [BurstCompile]
        private void Execute(Entity entity, ref DamageComponent damage, ref EnemyHpComponent hp)
        {
            //Debug.Log("DamageSystem - DamageJob");
            var resHp = hp.Hp - damage.Damage;
            var hpResult = new EnemyHpComponent { Hp = resHp, MaxHp = hp.MaxHp};
            Ecb.SetComponent(entity, hpResult);
            Ecb.SetComponentEnabled<DamageComponent>(entity, false);
        }
    }
}
