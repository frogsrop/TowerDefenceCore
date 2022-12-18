using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.LowLevel.Unsafe;
using UnityEngine;

class BulletAuthoring : UnityEngine.MonoBehaviour
{
    public List<AbstactEffectConfig> ListSo = new(); 
}

class BulletBaker : Baker<BulletAuthoring>
{
    private UnsafeUntypedBlobAssetReference s;
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