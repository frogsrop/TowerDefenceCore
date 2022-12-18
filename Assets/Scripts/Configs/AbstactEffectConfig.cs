using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEngine;

public abstract class AbstactEffectConfig : ScriptableObject
{
    public static Dictionary<int, AbstactEffectConfig> mapping = new();
    static int curId = 0;
    public int Id;

    private void OnEnable()
    {
        Id = curId;
        curId += 1;
        mapping[Id] = this;
        Debug.Log($"{name} -> {Id}");
    }
    public abstract void addBufferData(Entity e, EntityCommandBuffer ecb);
    public abstract void log();
}