using NUnit.Framework;
using System.Collections.Generic;
using Unity.Entities;
using UnityEditor.PackageManager;
using UnityEngine;

class BulletAuthoring : UnityEngine.MonoBehaviour
{
    public List<DamageBufferElementConfig> ListSo;
    public List<IBufferElementConfiguration<IBufferElementData>> listEf;
}

class BulletBaker : Baker<BulletAuthoring>
{

    public override void Bake(BulletAuthoring authoring)
    {
        for (int i = 0; authoring.ListSo.Count > i; i++)
        {
            var effect = authoring.ListSo[i].Damage;
            authoring.listEf.Add((IBufferElementConfiguration<IBufferElementData>)effect);
        }
        var newList = new List<IBufferElementData>();
       var test = authoring.listEf.Count;
        for (int i = 0; authoring.listEf.Count > i; i++)
        {
            var effect = authoring.listEf[i].getComponent();
            newList.Add(effect);
        }
        var bulletComponent = new BulletComponent{/*ListEffects = newList*/};

        //AddComponent(bulletComponent);
        AddComponent<TargetIdComponent>();
    }
}

