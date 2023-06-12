using Unity.Entities;
using UnityEngine;

class EnemyAuthoring : MonoBehaviour
{
    public int MaxHp = 500;
    public int Id = 0;
    public float Direction = 1;
    public float Speed = 5;
    public float SpeedModifier = 1.5;
    public float SpeedModifierTimer = 5;
    public float MaxSpeed = 10;
}

class EnemyBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
        AddComponent(new EnemyHpComponent { Hp = authoring.MaxHp, MaxHp = authoring.MaxHp });
        AddComponent(new DirectionComponent { Direction = authoring.Direction });
        AddComponent(new SpeedComponent { Speed = authoring.Speed, MaxSpeed = authoring.MaxSpeed });
        AddComponent(
            new SpeedModifierComponent { SpeedModifier = authoring.SpeedModifier, Timer = authoring.SpeedModifierTimer }
        );
        AddComponent(new EnemyIdComponent { Id = authoring.Id });
        AddComponent<TimerComponent>();
        AddComponent<DamageComponent>();
        AddComponent<BurningComponent>();
        AddBuffer<DamageBufferElement>();
        AddBuffer<BurningBufferElement>();
    }
}