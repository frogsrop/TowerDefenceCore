using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "Damage", menuName = "ScriptableObjects/DamageElementConfig", order = 1)]
public class DamageElementConfig : AbstactEffectConfig
{
    public int Damage;

    public override void addBufferData(Entity e, EntityCommandBuffer ecb)
    {
        Debug.Log($"Appending DAMAGE {Damage}");
        ecb.AppendToBuffer(e, new DamageBufferElement { Damage = Damage });
    }

    public override void log()
    {
        Debug.Log($"Damage = {Damage}");
    }
}