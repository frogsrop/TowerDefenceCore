using Unity.Entities;
using UnityEngine;

public class ManagerAuthoring : MonoBehaviour
{
    public GameObject TowerPrefab;
    public bool OnOff;
}

class ManagerBaker : Baker<ManagerAuthoring>
{
    public override void Bake(ManagerAuthoring authoring)
    {
        AddComponent(new PrefabComponent { TowerPrefab = GetEntity(authoring.TowerPrefab) });
        AddComponent(new SpawnComponent { OnOff = false });
    }
}