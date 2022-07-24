using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


[AddComponentMenu("DOTS Samples/IJobEntityBatch/Rotation Speed")]
[ConverterVersion("joe", 1)]
public class DirectionAuthoringComponent : MonoBehaviour, IConvertGameObjectToEntity
{
    public float direction;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var data = new DirectionComponent { direction = direction };
        dstManager.AddComponentData(entity, data);
    }
}

struct DirectionComponent : IComponentData
{
    public float direction;
}