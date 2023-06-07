using Unity.Entities;
using Unity.Mathematics;

public struct SpawnPostPayComponent : IComponentData
{
    public float3 TowerPos;
    public bool OnOff;
}