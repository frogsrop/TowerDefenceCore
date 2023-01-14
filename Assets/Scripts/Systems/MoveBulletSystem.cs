using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class MoveBulletSystem : SystemBase
{
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

    private static Dictionary<int, AbstractEffectConfig> _mapping;

    protected override void OnStartRunning()
    {
        _mapping = AbstractEffectConfig.Mapping;
    }

    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var queryEnemy = GetEntityQuery(ComponentType.ReadOnly<EnemyIdComponent>(),
            ComponentType.ReadOnly<LocalToWorldTransform>(),
            ComponentType.ReadOnly<DamageBufferElement>(),
            ComponentType.ReadOnly<BurningBufferElement>());
        var dt = SystemAPI.Time.DeltaTime;

        var enemyIds = queryEnemy.ToComponentDataArray<EnemyIdComponent>(Allocator.Temp);
        var enemyTransforms = queryEnemy.ToComponentDataArray<LocalToWorldTransform>(Allocator.Temp);
        var enemyEntityArray = queryEnemy.ToEntityArray(Allocator.Temp);
        if (enemyIds.Length > 0)
        {
            Entities.WithAll<TargetIdComponent>().ForEach(
                (ref LocalToWorldTransform bulletTransform, in TargetIdComponent bullet, in BulletComponent bulletInfo,
                    in Entity entity) =>
                {
                    var enemyIndex = IndexOf(enemyIds, bullet.Id);

                    if (enemyIndex != -1)
                    {
                        var enemyTransform = enemyTransforms[enemyIndex];
                        var enemyEntity = enemyEntityArray[enemyIndex];
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
                }).WithoutBurst().Run();
            Dependency.Complete();
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}