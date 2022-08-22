/*using Unity.Entities;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class PositionSystem : SystemBase
{

    protected override void OnUpdate()
    {
        Entities
            .WithAll<CreepTag>()
            .ForEach(( ref Translation translation, ref CreepPosComponent pos) =>
            {
                pos.creepPos = translation.Value;
                //var text = pos.creepPos.ToString();
                //Debug.Log(text);
            }).WithoutBurst().ScheduleParallel();

        Entities
            .WithAll<BulletSpawnTag>()
            .ForEach((ref Translation translation, ref SpawnPosComponent pos) =>
            {
                pos.spawnPos = translation.Value;
                var text = pos.spawnPos.ToString();
                Debug.Log(text);
            }).WithoutBurst().ScheduleParallel();
    }

}*/