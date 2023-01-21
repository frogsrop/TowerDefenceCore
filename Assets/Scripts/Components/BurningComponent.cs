using Unity.Entities;

public struct BurningComponent : IEnableableComponent, IComponentData
{
    public int BurningDamage;
    public float Timer;
}
