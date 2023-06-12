using Unity.Entities;

public struct SpeedComponent : IEnableableComponent, IComponentData
{
    public float Speed;
    public float MaxSpeed;
}
