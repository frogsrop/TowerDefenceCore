using System;
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
        var queryEnemy = GetEntityQuery(ComponentType.ReadOnly<EnemyIDComponent>(), 
            ComponentType.ReadOnly<LocalToWorldTransform>(),
            ComponentType.ReadOnly<DamageBufferElement>());
        var dt = SystemAPI.Time.DeltaTime;

        var enemyIds = queryEnemy.ToComponentDataArray<EnemyIDComponent>(Allocator.Temp);
        var enemyTransforms = queryEnemy.ToComponentDataArray<LocalToWorldTransform>(Allocator.Temp);
        var enemyArray = queryEnemy.ToEntityArray(Allocator.Temp);
        if (enemyIds.Length > 0)
        {
            Entities.WithStructuralChanges().WithAll<TargetIDComponent>().ForEach(
                (ref LocalToWorldTransform bulletTransform, in TargetIDComponent bullet, in Entity entity) =>
                {
                    var enemyIndex = enemyIds.IndexOf(bullet);
                    if (enemyIndex != -1)
                    {
                        var enemyTransform = enemyTransforms[enemyIndex];
                        var enemyEntity = enemyIds[enemyIndex];
                        var direction = math.normalize(enemyTransform.Value.Position - bulletTransform.Value.Position);
                        bulletTransform.Value.Position += direction * dt * 10;
                        var distance = math.distancesq(bulletTransform.Value.Position, enemyTransform.Value.Position);
                        if (distance < 0.1f)
                        {
                            EntityManager.DestroyEntity(entity);
                            ecb.AppendToBuffer(enemyArray[enemyIndex], new DamageBufferElement { damage = 3 });
                        }
                    }
                    else
                    {
                        EntityManager.DestroyEntity(entity);
                    }
                }).Run();
            Dependency.Complete();
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}