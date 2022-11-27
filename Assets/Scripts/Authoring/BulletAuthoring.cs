using NUnit.Framework;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEditor.PackageManager;
using UnityEngine;

class BulletAuthoring : UnityEngine.MonoBehaviour
{
    public List<DamageBufferElementConfig> ListSo = new();
}

class BulletBaker : Baker<BulletAuthoring>
{ 
    public override void Bake(BulletAuthoring authoring)
    {
        AddComponent<TargetIdComponent>();
        var list = new FixedList4096Bytes<TempTest>();
        foreach (var effect in authoring.ListSo)
        {
            list.Add(effect.DataHolder);
        }
        AddComponent(new BulletComponent { ListEffects = list });
    }
}