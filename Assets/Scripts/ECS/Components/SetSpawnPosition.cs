using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SetSpawnPosition : MonoBehaviour, IConvertGameObjectToEntity

{
    public Vector3 spawnPos;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var data = new SpawnPosComponent
        {
            spawnPos = spawnPos,
        };
        dstManager.AddComponentData(entity, data);
    }
}

struct SpawnPosComponent : IComponentData
{
    public float3 spawnPos;
}