using System;
using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class MoveBulletSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var queryEnemy = GetEntityQuery(ComponentType.ReadOnly<EnemyIdComponent>(), 
            ComponentType.ReadOnly<LocalToWorldTransform>(),
            ComponentType.ReadOnly<DamageBufferElement>());
        var dt = SystemAPI.Time.DeltaTime;

        var enemyIds = queryEnemy.ToComponentDataArray<EnemyIdComponent>(Allocator.Temp);
        var enemyTransforms = queryEnemy.ToComponentDataArray<LocalToWorldTransform>(Allocator.Temp);
        var enemyEntityArray = queryEnemy.ToEntityArray(Allocator.Temp);
        if (enemyIds.Length > 0)
        {
            Entities.WithAll<TargetIdComponent>().ForEach(
                (ref LocalToWorldTransform bulletTransform, in TargetIdComponent bullet, in Entity entity) =>
                {
                    var enemyIndex = enemyIds.IndexOf(bullet);
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
                            ecb.AppendToBuffer(enemyEntity, new DamageBufferElement { damage = 3 });
                            var log = EntityManager.IsComponentEnabled<DamageComponent>(enemyEntity);
                            UnityEngine.Debug.Log(log);
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