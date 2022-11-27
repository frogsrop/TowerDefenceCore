using System.Security;
using Unity.Entities;
using UnityEngine;

[InternalBufferCapacity(999)]
public struct DamageBufferElement : IBufferElementConfiguration<TempTest>
{
    public int Damage;

    public IBufferElementData BuildComponent(TempTest data)
    {
        return new DamageBufferElement { Damage = data.Damage };
    }
}r

public struct TempTest
{
    public int Damage;
}