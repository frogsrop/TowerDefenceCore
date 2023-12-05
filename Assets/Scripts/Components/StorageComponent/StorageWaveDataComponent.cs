using Unity.Entities;
using UnityEngine;

public struct StorageWaveDataComponent : IComponentData
{
    public int WaveLength;
    public int FullWaveLength;
    public bool StopWave;
}