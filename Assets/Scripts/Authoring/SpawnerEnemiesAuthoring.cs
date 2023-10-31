using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

class SpawnerEnemiesAuthoring : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float SpeedSpawn = 2;
}

class TestSpawnerBaker : Baker<SpawnerEnemiesAuthoring>
{
    public override void Bake(SpawnerEnemiesAuthoring enemiesAuthoring)
    {
        AddComponent(new SpawnerEnemiesComponent { EnemyPrefab = GetEntity(enemiesAuthoring.EnemyPrefab), 
            SpeedSpawn = enemiesAuthoring.SpeedSpawn });
        AddComponent(new SpawnCountEnemiesComponent { Count = 0 });
        AddComponent<TimerComponent>();
    }
}