using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public abstract class AbstactEffectConfig : ScriptableObject
{
    public static Dictionary<int, AbstactEffectConfig> Mapping = new();
    static int CurId = 0;
    public int Id;

    private void OnEnable()
    {
        Id = CurId;
        CurId += 1;
        Mapping[Id] = this;
        Debug.Log($"{name} -> {Id}");
    }
    public abstract void addBufferData(Entity e, EntityCommandBuffer ecb);
    public abstract void log();
}