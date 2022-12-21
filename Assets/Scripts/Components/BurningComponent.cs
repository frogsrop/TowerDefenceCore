using Unity.Entities;

public struct BurningComponent : IEnableableComponent, IComponentData
{
    public float Timer;
    public int BurningDamage;
}
