using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
// public class DirectionAuthoringComponent : MonoBehaviour, IConvertGameObjectToEntity
// {
//     public float direction;
//
//     public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
//     {
//         var data = new DirectionComponent { Direction = direction };
//         dstManager.AddComponentData(entity, data);
//     }
// }

[GenerateAuthoringComponent]
struct DirectionComponent : IComponentData
{
    public float Direction;
}