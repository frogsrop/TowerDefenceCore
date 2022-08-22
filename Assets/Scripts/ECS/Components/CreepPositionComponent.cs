using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class CreepPositionComponent : MonoBehaviour, IConvertGameObjectToEntity
{
    public Vector3 creepPos;
      public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var data = new CreepPosComponent 
        { 
            creepPos = creepPos,
        };
        dstManager.AddComponentData(entity, data);
    }
}

struct CreepPosComponent : IComponentData
{
    public float3 creepPos; 
}