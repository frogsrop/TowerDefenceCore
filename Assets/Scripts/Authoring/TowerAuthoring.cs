using Unity.Entities;
using UnityEngine;

class TowerAuthoring : MonoBehaviour
{
    public GameObject BulletPrefab;
}

class TowerBaker : Baker<TowerAuthoring>
{
    public override void Bake(TowerAuthoring authoring)
    {
        AddComponent(new Tower { BulletPrefab = GetEntity(authoring.BulletPrefab) });
        AddComponent<TimerComponent>();
        AddComponent<TowerSpeedAttack>();
    }
}