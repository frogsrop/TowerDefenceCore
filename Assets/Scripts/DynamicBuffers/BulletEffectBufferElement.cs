using Unity.Entities;

[InternalBufferCapacity(999)]
public struct TakeDamageBufferElement : IBufferElementData
{
    public float Value;
}
