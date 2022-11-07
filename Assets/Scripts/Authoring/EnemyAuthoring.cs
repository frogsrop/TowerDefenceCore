using Unity.Entities;

class EnemyAuthoring : UnityEngine.MonoBehaviour
{
    public int id = 0;
    public float direction = 5;
}

class EnemyBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
        AddComponent<Enemy>();
        AddComponent(new DirectionComponent { Direction = authoring.direction });
        AddComponent(new IDEnemy { Id = authoring.id });
        AddBuffer<TakeDamageBufferElement>();
    }
}