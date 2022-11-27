using System;
using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
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
                            ecb.AppendToBuffer(enemyEntity, new DamageBufferElement { Damage = 3 });
                        }
                    }
                    else
                    {
                        ecb.DestroyEntity(entity);
                        UnityEngine.Debug.Log("CrashBullet");
                    }
                }).WithoutBurst().Run();
            Dependency.Complete();
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}