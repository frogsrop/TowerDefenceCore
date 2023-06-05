using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    public int MaxHp = 500;
    public int Id = 0;
    public float Direction = 5;
    
    public float Speed;
    public GameObject Prefab;
}

public class EnemyBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
        AddComponent(new EnemyHpComponent { Hp = authoring.MaxHp, MaxHp = authoring.MaxHp });
        AddComponent(new DirectionComponent { Direction = authoring.Direction });
        AddComponent(new EnemyIdComponent { Id = authoring.Id });
        AddComponent<TimerComponent>();
        AddComponent<DamageComponent>();
        AddComponent<BurningComponent>();
        AddBuffer<DamageBufferElement>();
        AddBuffer<BurningBufferElement>();
        if (authoring.Speed > 0)
        {
            SpeedComponent speed = default;
            speed.Value = authoring.Speed;
            AddComponent(speed);
        }

        PresentationGoComponent pgo = new PresentationGoComponent();
        pgo.Prefab = authoring.Prefab;
        AddComponentObject(pgo);
    }
}
