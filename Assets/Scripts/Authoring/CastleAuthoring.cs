using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CastleAuthoring : MonoBehaviour
{
    public List<float3> Path;
}

class CastleAuthoringBaker : Baker<CastleAuthoring>
{
    public override void Bake(CastleAuthoring authoring)
    {
        var castleEntity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent<CastleComponent>(castleEntity);
        AddComponent<OffSceneComponent>(castleEntity);

        DynamicBuffer<WayPointsComponent> path = AddBuffer<WayPointsComponent>(castleEntity);
        foreach (var point in authoring.Path)
        {
            WayPointsComponent wp = default;
            wp.Value = point;
            path.Add(wp);
        }
    }
}