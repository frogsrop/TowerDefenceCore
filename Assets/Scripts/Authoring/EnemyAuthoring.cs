using Unity.Entities;

class EnemyAuthoring : UnityEngine.MonoBehaviour
{
    public int MaxHp = 500;
    public int Id = 0;
    public float Direction = 5;
}

class EnemyBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
        AddComponent(new EnemyHpComponent { Hp = authoring.MaxHp});
        AddComponent(new DirectionComponent { Direction = authoring.Direction });
        AddComponent(new EnemyIdComponent { Id = authoring.Id });
        AddComponent<DamageComponent>();
        AddBuffer<DamageBufferElement>();
    }
}