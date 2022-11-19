using System;
using Unity.Entities;
public struct EnemyIdComponent : IComponentData, IEquatable<EnemyIdComponent>, IEquatable<TargetIdComponent>
{
    public int Id;
    
    public bool Equals(EnemyIdComponent other)
    {
        return Id == other.Id;
    }

    public bool Equals(TargetIdComponent other)
    {
        return Id == other.Id;
    }
}

