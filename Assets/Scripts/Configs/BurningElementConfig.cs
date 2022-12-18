using Unity.Entities;
using UnityEngine;

[CreateAssetMenu(fileName = "Burn", menuName = "ScriptableObjects/BurningElementConfig", order = 1)]
public class BurningElementConfig : AbstactEffectConfig
{
    public int Damage;
    public float Timer;

    public override void addBufferData(Entity e, EntityCommandBuffer ecb)
    {
        ecb.AppendToBuffer(e, new BurningBufferElement { Damage = Damage, Timer = Timer });
    }


    public override void log()
    {
        Debug.Log($"Damage = {Damage}, Timer = {Timer}");
    }
}