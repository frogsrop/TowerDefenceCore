using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

class BulletAuthoring : MonoBehaviour
{
    public List<AbstractEffectConfig> ListSo = new(); 
}

class BulletBaker : Baker<BulletAuthoring>
{
    public override void Bake(BulletAuthoring authoring)
    {
        
        AddComponent<TargetIdComponent>();
        var list = new FixedList4096Bytes<int>();
        foreach (var effect in authoring.ListSo)
        {
            list.Add(effect.Id);
        }
        AddComponent(new BulletComponent { ListEffects = list });
    }
}