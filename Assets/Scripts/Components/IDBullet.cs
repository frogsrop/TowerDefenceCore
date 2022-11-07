using System;
using Unity.Entities;

public struct IDBullet : IComponentData, IEquatable<IDEnemy>, IEquatable<IDBullet>
{
    public int Id;

    public bool Equals(IDEnemy other)
    {
        return Id == other.Id;
    }

    public bool Equals(IDBullet other)
    {
        return Id == other.Id;
    }
}