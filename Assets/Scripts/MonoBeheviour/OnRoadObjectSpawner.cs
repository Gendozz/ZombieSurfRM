using UnityEngine;

/// <summary>
/// Spawns obstacles and all collectibles (which are positioning based on CellFrame)
/// </summary>
public class OnRoadObjectSpawner : Spawner
{
    private IObjectGenerator generator;

    private int mapWidth = 3;

    private int mapLength = 30;

    private float difficulty = 0.5f;

    [Header("Generator type based on:")]
    public generatorType type;

    public enum generatorType
    {
        MAP,
        DELAY
    }

    public override void StartSpawn()
    {
        switch (type)
        {
            case generatorType.MAP:
                generator = new OnRoadObjectMapGenerator(mapWidth, mapLength, difficulty, firstObjectSpawnPosition);
                break;
            case generatorType.DELAY:
                generator = new OnRoadDelayedObjectGenerator(firstObjectSpawnPosition, mapWidth);
                break;
            default:
                Debug.LogError("No OnRoad generator found");
                break;
        }

        base.StartSpawn();
    }

    public override void AddObject()
    {
        Vector3 positionToSpawn = generator.GetPositionToSpawn();
        pooler.SpawnFromPool(poolTagToSpawnFrom.GetValue(), positionToSpawn);
    }
}


