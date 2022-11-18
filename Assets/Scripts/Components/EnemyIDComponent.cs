using System;
using Unity.Entities;
public struct EnemyIDComponent : IComponentData, IEquatable<EnemyIDComponent>, IEquatable<TargetIDComponent>
{
    public int Id;
    
    public bool Equals(EnemyIDComponent other)
    {
        return Id == other.Id;
    }

    public bool Equals(TargetIDComponent other)
    {
        return Id == other.Id;
    }
}

