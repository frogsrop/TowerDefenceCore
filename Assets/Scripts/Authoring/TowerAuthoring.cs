using Unity.Entities;
using static UnityEngine.GraphicsBuffer;

class TowerAuthoring : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject BulletPrefab;
}

class TowerBaker : Baker<TowerAuthoring>
{
    public override void Bake(TowerAuthoring authoring)
    {
        AddComponent(new Tower
        {
            BulletPrefab = GetEntity(authoring.BulletPrefab)
        });
    }
}
