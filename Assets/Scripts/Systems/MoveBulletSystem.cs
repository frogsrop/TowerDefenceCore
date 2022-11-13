using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.SocialPlatforms;

public partial class MoveBulletSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var queryEnemy = GetEntityQuery(ComponentType.ReadOnly<Enemy>(),
            ComponentType.ReadOnly<LocalToWorldTransform>(),
            ComponentType.ReadOnly<IDEnemy>());
        var dt = SystemAPI.Time.DeltaTime;
        var enemyIds = queryEnemy.ToComponentDataArray<IDEnemy>(Allocator.Temp);
        var enemyTransforms = queryEnemy.ToComponentDataArray<LocalToWorldTransform>(Allocator.Temp);
        if (enemyIds.Length > 0)
        {
            Entities.WithStructuralChanges().WithAll<IDBullet>().ForEach(
                (ref LocalToWorldTransform bulletTransform, in IDBullet bullet, in Entity entity) =>
                {
                    var enemyIndex = enemyIds.IndexOf(bullet);
                    if (enemyIndex != -1)
                    {
                        var enemyTransform = enemyTransforms[enemyIndex];
                        var direction = math.normalize(enemyTransform.Value.Position - bulletTransform.Value.Position);
                        bulletTransform.Value.Position += direction * dt * 10;
                        var distance = math.distancesq(bulletTransform.Value.Position, enemyTransform.Value.Position);
                        if (distance < 0.1f)
                        {
                            EntityManager.DestroyEntity(entity);
                        }
                    }
                    else
                    {
                        EntityManager.DestroyEntity(entity);
                    }
                }).Run();
        }
    }
}