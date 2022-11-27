using Unity.Entities;
using UnityEngine;

[InternalBufferCapacity(999)]
public struct DamageBufferElement : IBufferElementData
{
    public int Damage;
}



[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DamageBufferElementConfig", order = 1)]
public class DamageBufferElementConfig : ScriptableObject, IBufferElementConfiguration<DamageBufferElement>
{
    public int Damage;

    public DamageBufferElement getComponent()
    {
        return new DamageBufferElement { Damage = Damage };
    }
}




interface IBufferElementConfiguration<T>
{

    T getComponent();
}