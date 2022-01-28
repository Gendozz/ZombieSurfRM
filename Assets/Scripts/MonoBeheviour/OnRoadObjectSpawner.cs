using UnityEngine;

/// <summary>
/// Spawns obstacles and all collectibles (which are positioning based on CellFrame)
/// </summary>
public class OnRoadObjectSpawner : Spawner
{
    private IObjectGenerator generator;

    private int mapWidth = 3;

    private int mapLength = 30;
    
    [SerializeField]
    private float difficulty = 1f;

    [Header("Generator type based on:"), Tooltip("MAP - 3 lanes, based on back cut\n" +
        "DELAY - 3 lanes, random lane, random distance\n" +
        "LINEAR - 1 lane, for decorartions")]
    public generatorType type;

    public enum generatorType
    {
        MAP,
        DELAY,
        LINEAR
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
            case generatorType.LINEAR:
                generator = new OnRoadDelayedObjectGenerator(firstObjectSpawnPosition, 0);
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


