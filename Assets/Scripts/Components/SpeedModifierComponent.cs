using Unity.Entities;

public struct SpeedModifierComponent : IEnableableComponent, IComponentData
{
    public float ModifierCoefficient;
    public float Timeout;
}
