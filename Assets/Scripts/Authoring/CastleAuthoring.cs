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
        AddComponent<CastleComponent>();
        
        DynamicBuffer<WayPointsComponent> path = AddBuffer<WayPointsComponent>();
        foreach(var point in authoring.Path)
        {
            WayPointsComponent wp = default;
            wp.Value = point;
            path.Add(wp);
        }
    }
}
