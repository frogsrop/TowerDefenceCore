using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class NearestTargetingStrategy : ITargetingStrategy
{
    private EntityManager _entityManager;

    public NearestTargetingStrategy(EntityManager entityManager)
    {
        _entityManager = entityManager;
    }

    public Entity ChooseTarget(Entity tower, EntityQuery enemiesQuery)
    {
        var enemiesTransforms = enemiesQuery.ToComponentDataArray<LocalToWorldTransform>(Allocator.Temp);
        var towerPosition = _entityManager.GetComponentData<LocalToWorldTransform>(tower).Value.Position;
        int minDistanceIndex = -1;
        for (var i = 0; i < enemiesTransforms.Length; i++)
        {
            var distanceq = math.distancesq(towerPosition, enemiesTransforms[i].Value.Position);
            if (minDistanceIndex == -1 ||
                distanceq <= math.distancesq(towerPosition, enemiesTransforms[minDistanceIndex].Value.Position))
            {
                minDistanceIndex = i;
            }
        }

        return enemiesQuery.ToEntityArray(Allocator.Temp)[minDistanceIndex];
    }
}