using System;
using Unity.Entities;
using UnityEngine;



[CreateAssetMenu(fileName = "Damage", menuName = "ScriptableObjects/DamageElementConfig", order = 1)]
public class DamageEffectConfig : AbstractConvertableConfig<DamageBufferElement> 
{
    public int Damage;
}