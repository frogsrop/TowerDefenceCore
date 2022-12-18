using System.Security;
using Unity.Entities;
using UnityEngine;

[InternalBufferCapacity(999)]
public struct BurningBufferElement: IBufferElementData
{
    public int Damage;
    public float Timer;
}
