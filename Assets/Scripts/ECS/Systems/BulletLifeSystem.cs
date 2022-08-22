/*using Unity.Entities;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

public partial class BulletFlySystem : SystemBase
{

    private Entity BulletPrefab;
    private EntityQuery BulletQuery;
    protected override void OnCreate()
    {
        BulletQuery = GetEntityQuery(ComponentType.ReadWrite<BulletTag>());
        BulletPrefab = GetSingleton<BulletPrefab>().Value;
    }
    protected override void OnUpdate() 
            
    {
        
        
        
        
        
        
        Entities
            .WithAll<BulletTag>()
            .ForEach((ref Translation translation, ref CreepPosComponent pos) =>
            {
                var posBullet = translation.Value;
                var posCreep = pos.creepPos;


                if (posCreep != posBullet)
                {

                }
                var text = pos.creepPos.ToString();
                Debug.Log(text);
            }).WithoutBurst().ScheduleParallel();
    }
}

//// We create the bullet here
//var bulletEntity = commandBuffer.Instantiate(entityInQueryIndex, bulletPrefab);

////we set the bullets position as the player's position + the bullet spawn offset
////math.mul(rotation.Value,bulletOffset.Value) finds the position of the bullet offset in the given rotation
////think of it as finding the LocalToParent of the bullet offset (because the offset needs to be rotated in the players direction)

//var newPosition = new Translation { Value = position.Value + math.mul(rotation.Value, bulletOffset.Value).xyz };
//commandBuffer.SetComponent(entityInQueryIndex, bulletEntity, newPosition);

//Here we set the prefab we will use
if (m_PlayerPrefab == Entity.Null || m_BulletPrefab == Entity.Null)
{
    //We grab the converted PrefabCollection Entity's PlayerAuthoringCOmponent
    //and set m_PlayerPrefab to its Prefab value
    m_PlayerPrefab = GetSingleton<PlayerAuthoringComponent>().Prefab;
    m_BulletPrefab = GetSingleton<BulletAuthoringComponent>().Prefab;

    //we must "return" after setting this prefab because if we were to continue into the Job
    //we would run into errors because the variable was JUST set (ECS funny business)
    //comment out return and see the error
    return;
}*/