using UnityEngine;

/// <summary>
/// Spawns obstacles and all collectibles (which are positioning based on CellFrame)
/// </summary>
public class OnRoadObjectSpawner : Spawner
{
    private OnRoadObjectMapGenerator objectMap;

    private int mapWidth = 3;

    private int mapLength = 30;

    private float difficulty = 0.5f;

    public override void StartSpawn()
    {
        objectMap = new OnRoadObjectMapGenerator(mapWidth, mapLength, difficulty, firstObjectSpawnPosition);
        base.StartSpawn();
    }

    public override void AddObject()
    {
        Vector3 positionToSpawn = objectMap.GetPositionToSpawn();
        pooler.SpawnFromPool(poolTagToSpawnFrom.GetValue(), positionToSpawn);        
    }
}
