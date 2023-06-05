using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

[BurstCompile]
public partial struct SpawnTowerOnGridSystem : ISystem
{
    public void OnCreate(ref SystemState state) { }
    [BurstCompile] public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        new SpawnEntitiesJob { Ecb = ecb, Dt = dt}.Run();
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

public partial struct SpawnEntitiesJob : IJobEntity
{
    public EntityCommandBuffer Ecb;
    public float Dt;
    private void Execute(Entity e, in PrefabComponent prefabComponent, ref SpawnComponent spawnComponent)
    {
        if(!spawnComponent.OnOff) return;
        var posTowerX = Mathf.Round(spawnComponent.TowerPos.x / 2) * 2;
        var posTowerY = Mathf.Round(spawnComponent.TowerPos.y / 2) * 2;
        var posTowerSpawn = new float3(posTowerX, posTowerY, 0);
        var towerUniformScaleTransform = new UniformScaleTransform
            { Position = posTowerSpawn, Scale = 0.5f };
        var setSpawnTowerPosition = new LocalToWorldTransform
            { Value = towerUniformScaleTransform };
        var eTower = Ecb.Instantiate(prefabComponent.TowerPrefab);
        Ecb.SetComponent(eTower, setSpawnTowerPosition);
        
        var offSpawn = new SpawnComponent { OnOff = false };
        Ecb.SetComponent(e, offSpawn);
    }
}