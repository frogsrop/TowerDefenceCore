using Unity.Entities;

public struct SpeedModifierComponent : IEnableableComponent, IComponentData
{
    public float Speed;
    public float MaxSpeed;
}
