using Unity.Entities;

[InternalBufferCapacity(999)]
public struct DamageBufferElement : IBufferElementData
{
    public int damage;
}
