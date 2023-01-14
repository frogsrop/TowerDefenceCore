using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public abstract class AbstractEffectConfig : ScriptableObject
{
    public static Dictionary<int, AbstractEffectConfig> Mapping = new();
    private static int CurId;
    private int _id;
    public int Id => _id;

    private void OnEnable()
    {
        _id = CurId;
        CurId++;
        Mapping[_id] = this;
    }

    public abstract void AppendToBuffer(Entity entity, EntityCommandBuffer ecb);

}