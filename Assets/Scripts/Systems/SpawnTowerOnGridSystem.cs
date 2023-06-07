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
    private void Execute(Entity e, in PrefabComponent prefabComponent, ref SpawnPostPayComponent spawnPostPayComponent)
    {
        if(!spawnPostPayComponent.OnOff) return;
        var posTowerSpawn = new float3(spawnPostPayComponent.TowerPos.x, spawnPostPayComponent.TowerPos.y, 0);
        var towerUniformScaleTransform = new UniformScaleTransform
            { Position = posTowerSpawn, Scale = 0.5f };
        var setSpawnTowerPosition = new LocalToWorldTransform
            { Value = towerUniformScaleTransform };
        var eTower = Ecb.Instantiate(prefabComponent.Prefab);
        Ecb.SetComponent(eTower, setSpawnTowerPosition);
        
        var offSpawn = new SpawnPostPayComponent { OnOff = false };
        Ecb.SetComponent(e, offSpawn);
    }
}