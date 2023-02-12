using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct DamageSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var queryDamageComponent = state.GetEntityQuery(ComponentType.ReadWrite<DamageComponent>());
        new OffDamageComponentJob{ecbJob = ecb}.Run(queryDamageComponent);
    }
    
    [BurstCompile] public void OnDestroy(ref SystemState state) {}

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var em = state.EntityManager;
        new DamageJob
        {
            em=em,
            ecbJob = ecb
        }.Run();
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
    public partial struct OffDamageComponentJob : IJobEntity
    {
        
        public EntityCommandBuffer ecbJob;
        private void Execute(Entity entity, ref DamageComponent damage)
        {
            ecbJob.SetComponentEnabled<DamageComponent>(entity, false); /*TODO: when a creep spawns*/
        }
    }
    
    public partial struct DamageJob : IJobEntity
    {
        public EntityManager em;
        public EntityCommandBuffer ecbJob;
        private void Execute(Entity entity, ref DamageComponent damage, ref EnemyHpComponent hp)
        {
            
            var resHp = hp.Hp - damage.Damage;
            var hpResult = new EnemyHpComponent { Hp = resHp , MaxHp = hp.MaxHp};
            ecbJob.SetComponent(entity, hpResult);
            ecbJob.SetComponentEnabled<DamageComponent>(entity, false);
        }
    }
}
