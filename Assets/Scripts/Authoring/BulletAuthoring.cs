using Unity.Entities;

class BulletAuthoring : UnityEngine.MonoBehaviour
{
}

class BulletBaker : Baker<BulletAuthoring>
{
    public override void Bake(BulletAuthoring authoring)
    {
        AddComponent<TargetIdComponent>();
    }
}