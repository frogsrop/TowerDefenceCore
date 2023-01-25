using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


public partial class MoveBulletSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var targetIdQuery = GetEntityQuery(ComponentType.ReadOnly<TargetIdComponent>());
         var queryEnemy = GetEntityQuery(ComponentType.ReadOnly<EnemyIdComponent>(),
             ComponentType.ReadOnly<LocalToWorldTransform>(),
             ComponentType.ReadOnly<DamageBufferElement>(),
             ComponentType.ReadOnly<BurningBufferElement>());
         var enemyIds = queryEnemy.ToComponentDataArray<EnemyIdComponent>(Allocator.TempJob);
         var ecb = new EntityCommandBuffer(Allocator.TempJob);
         if (enemyIds.Length > 0)
         {
             var dt = SystemAPI.Time.DeltaTime;
             new MoveBulletJob
             {
                 dtJob = dt,
                 ecbJob = ecb,
                 enemyIdsInJob = enemyIds,
                 enemyTransformsInJob = queryEnemy.ToComponentDataArray<LocalToWorldTransform>(Allocator.TempJob),
                 enemyEntityArrayInJob = queryEnemy.ToEntityArray(Allocator.TempJob)
             }.Run(targetIdQuery);
             Dependency.Complete();
             ecb.Playback(EntityManager);
             ecb.Dispose();
        }
    }
}

public partial struct MoveBulletJob : IJobEntity
{
    public float dtJob;
    public EntityCommandBuffer ecbJob;
    public NativeArray<EnemyIdComponent> enemyIdsInJob;
    public NativeArray<LocalToWorldTransform> enemyTransformsInJob;
    public NativeArray<Entity> enemyEntityArrayInJob;
    
    public void Execute(ref LocalToWorldTransform bulletTransform, in TargetIdComponent bullet, in BulletComponent bulletInfo,
        in Entity entity)
    {
        var mapping = AbstractEffectConfig.Mapping;
        var enemyIndex = IndexOf(enemyIdsInJob, bullet.Id);
        if (enemyIndex != -1)
        {
            var enemyTransform = enemyTransformsInJob[enemyIndex];
            var enemyEntity = enemyEntityArrayInJob[enemyIndex];
            var direction = math.normalize(enemyTransform.Value.Position - bulletTransform.Value.Position);
            bulletTransform.Value.Position += direction * dtJob * 10;
            var distance = math.distancesq(bulletTransform.Value.Position, enemyTransform.Value.Position);
            if (distance < 0.1f)
            {
                ecbJob.DestroyEntity(entity);
                foreach (var effect in bulletInfo.ListEffects)
                {
                    if (mapping.ContainsKey(effect))
                    {
                        mapping[effect].AppendToBuffer(enemyEntity, ecbJob);
                    }
                }
            }
        }
        else
        {
            ecbJob.DestroyEntity(entity);
        }
    }
    
    int IndexOf(NativeArray<EnemyIdComponent> array, int id)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].Id == id)
            {
                return i;
            }
        }

        return -1;
    }
}