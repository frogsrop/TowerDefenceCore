using System.Security;
using Unity.Entities;
using UnityEngine;

[InternalBufferCapacity(999)]
public struct BurningBufferElement : IBufferElementConfiguration<TempTest>
{
    public int Damage;
    public float Timer;

    public IBufferElementData BuildComponent(TempTest data)
    {
        return new BurningBufferElement { Damage = data.Damage, Timer = data.Timer };
    }
}
