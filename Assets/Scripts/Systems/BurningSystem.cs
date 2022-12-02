using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial class BurningSystem : SystemBase
{
    protected override void OnStartRunning()
    {
        Entities.WithAll<DamageComponent>()
            .ForEach(
                (Entity entity, ref DamageComponent damage) =>
                {
                    EntityManager.SetComponentEnabled<BurningComponent>(entity, false); //TODO: when a creep spawns
                }).WithoutBurst().Run();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        
    }
}