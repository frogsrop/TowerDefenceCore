using Unity.Entities;

public struct SpeedModifierComponent : IEnableableComponent, IComponentData
{
    public int ModifierCoefficient;
    public float Timeout;
}
