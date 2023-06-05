using Unity.Entities;
using Unity.Mathematics;

public struct SpawnComponent : IComponentData
{
    public float3 TowerPos;
    public bool OnOff;
}