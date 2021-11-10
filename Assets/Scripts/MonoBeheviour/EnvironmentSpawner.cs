using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnvironmentSpawner : Spawner
{

    protected override void AddObject()
    {
        Vector3 newPosition = lastSpawnedObject.GetComponent<ReplacableObject>().endPosition.position;
        lastSpawnedObject = pooler.SpawnFromPool(poolTagToSpawnFrom.GetValue(), newPosition);
    }
}
