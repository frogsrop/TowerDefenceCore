using System.Security;
using Unity.Entities;
using UnityEngine;

[InternalBufferCapacity(999)]
public struct DamageBufferElement : IBufferElementData
{
    public int Damage;
}

