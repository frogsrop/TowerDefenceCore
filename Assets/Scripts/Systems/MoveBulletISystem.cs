using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


[BurstCompile]
public partial struct MoveBulletISystem : ISystem
{
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        throw new System.NotImplementedException();
    }
    
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        throw new System.NotImplementedException();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var targetIdQuery = state.GetEntityQuery(ComponentType.ReadOnly<TargetIdComponent>());
        var queryEnemy = state.GetEntityQuery(ComponentType.ReadOnly<EnemyIdComponent>(),
            ComponentType.ReadOnly<LocalToWorldTransform>(),
            ComponentType.ReadOnly<DamageBufferElement>(),
            ComponentType.ReadOnly<BurningBufferElement>());
        var enemyIds = queryEnemy.ToComponentDataArray<EnemyIdComponent>(Allocator.Temp);

        if (enemyIds.Length > 0)
        {
            new MoveBulletJob
            {
                dt = SystemAPI.Time.DeltaTime,
                ecb = new EntityCommandBuffer(Allocator.TempJob),
                enemyIdsInJob = enemyIds;
                enemyTransformsInJob = queryEnemy.ToComponentDataArray<LocalToWorldTransform>(Allocator.Temp);
                enemyEntityArrayInJob = queryEnemy.ToEntityArray(Allocator.Temp);
            }.Run(targetIdQuery);

        }

    }


    public void OnCreateForCompiler(ref SystemState state)
    {
        throw new System.NotImplementedException();
    }
}

[BurstCompile]
public partial struct MoveBulletJob : IJobEntity
{
    public float dt;
    public static Dictionary<int, AbstractEffectConfig> _mapping = AbstractEffectConfig.Mapping;
    public EntityCommandBuffer ecb;
    public NativeArray<EnemyIdComponent> enemyIdsInJob;
    public NativeArray<LocalToWorldTransform> enemyTransformsInJob;
    public NativeArray<Entity> enemyEntityArrayInJob;
    
    public void Execute(ref LocalToWorldTransform bulletTransform, in TargetIdComponent bullet, in BulletComponent bulletInfo,
        in Entity entity)
    {
        var enemyIndex = IndexOf(enemyIdsInJob, bullet.Id);
        if (enemyIndex != -1)
        {
            var enemyTransform = enemyTransformsInJob[enemyIndex];
            var enemyEntity = enemyEntityArrayInJob[enemyIndex];
            var direction = math.normalize(enemyTransform.Value.Position - bulletTransform.Value.Position);
            bulletTransform.Value.Position += direction * dt * 10;
            var distance = math.distancesq(bulletTransform.Value.Position, enemyTransform.Value.Position);
            if (distance < 0.1f)
            {
                ecb.DestroyEntity(entity);
                foreach (var effect in bulletInfo.ListEffects)
                {
                    if (_mapping.ContainsKey(effect))
                    {
                        _mapping[effect].AppendToBuffer(enemyEntity, ecb);
                    }
                }
            }
        }
        else
        {
            ecb.DestroyEntity(entity);
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