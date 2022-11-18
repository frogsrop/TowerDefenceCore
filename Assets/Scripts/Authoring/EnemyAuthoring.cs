using Unity.Entities;

class EnemyAuthoring : UnityEngine.MonoBehaviour
{
    public int hp = 500;
    public int id = 0;
    public float direction = 5;
}

class EnemyBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
        AddComponent(new EnemyHPComponent { hp = authoring.hp});
        AddComponent(new DirectionComponent { Direction = authoring.direction });
        AddComponent(new EnemyIDComponent { Id = authoring.id });
        AddComponent<DamageComponent>();
        AddBuffer<DamageBufferElement>();
    }
}