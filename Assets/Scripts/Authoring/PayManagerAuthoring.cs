using Unity.Entities;
using UnityEngine;

public class PayManagerAuthoring : MonoBehaviour
{
    public GameObject TowerPrefab;
    public bool OnOff;
}

class PayManagerBaker : Baker<PayManagerAuthoring>
{
    public override void Bake(PayManagerAuthoring authoring)
    {
        AddComponent(new PrefabComponent { Prefab = GetEntity(authoring.TowerPrefab) });
        AddComponent(new SpawnPostPayComponent { OnOff = false });
    }
}