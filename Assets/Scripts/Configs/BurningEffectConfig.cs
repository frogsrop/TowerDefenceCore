using Unity.Entities;
using UnityEngine;

[CreateAssetMenu(fileName = "Burn", menuName = "ScriptableObjects/BurningElementConfig", order = 1)]
public class BurningEffectConfig : AbstractConvertableConfig<BurningBufferElement>
{
    public int Damage;
    public float Timer;
}