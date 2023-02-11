using Unity.Entities;

public struct TimerComponent : IEnableableComponent, IComponentData
{
    public float Time;
    public bool Trigger;
    public bool Condition;
}
