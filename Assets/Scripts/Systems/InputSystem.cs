/*using Unity.Entities;
using UnityEngine;

public partial class InputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (Input.GetKey(KeyCode.S))
        {
            Entities
                .WithAll<EnemyHPComponent>()
                .WithAll<DamageBufferElement>()
                .WithDeferredPlaybackSystem<BeginSimulationEntityCommandBufferSystem>()
                .ForEach((Entity entity, EntityCommandBuffer ecb) =>
                {
                    ecb.AppendToBuffer(entity, new DamageBufferElement { damage = 3 });
                    Debug.Log("Damage: 3");
                }).Run();

            Dependency.Complete();
        }
    }
}*/