using Unity.Entities;
using UnityEngine;

public class TestResAuthoring : MonoBehaviour
{
    public GameObject TowerPrefab;
    public GameObject EnemyPrefab;
}

class TestResBaker : Baker<TestResAuthoring>
{
    public override void Bake(TestResAuthoring authoring)
    {
        AddComponent(new TestComponent {
            TowerPrefab = GetEntity(authoring.TowerPrefab),
            EnemyPrefab = GetEntity(authoring.EnemyPrefab)
        });
        AddComponent<QuantitySpawnComponent>();
    }
}