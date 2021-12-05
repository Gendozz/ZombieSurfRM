using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns environment (road, buldings, decoration etc.) (which are positioning seamlessly 
/// based on start-to-end technic)
/// </summary>
public class EnvironmentSpawner : Spawner
{
    public override void AddObject()
    {
        Vector3 newPosition = lastSpawnedObject.GetComponent<ReplacableObject>().endPosition.position;
        lastSpawnedObject = pooler.SpawnFromPool(poolTagToSpawnFrom.GetValue(), newPosition);
    }
}
