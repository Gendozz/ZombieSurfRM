using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns environment (road, buldings, decorations etc.) (which are positioning seamlessly 
/// based on start-to-end technic)
/// </summary>
public class EnvironmentSpawner : Spawner
{
    private GameObject lastSpawnedObject = null;

    protected override void Init()
    {
        lastSpawnedObject = pooler.SpawnFromPool(poolTagToSpawnFrom.GetValue(), firstObjectSpawnPosition);

        base.Init();
    }

    public override void AddObject()
    {
        Vector3 newPosition = lastSpawnedObject.GetComponent<ReplacableObject>().endPosition.position;
        lastSpawnedObject = pooler.SpawnFromPool(poolTagToSpawnFrom.GetValue(), newPosition);        
    }
}
