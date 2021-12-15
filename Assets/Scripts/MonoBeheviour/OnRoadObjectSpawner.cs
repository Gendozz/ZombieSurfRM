using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns obstacles and all collectibles (which are positioning based on CellFrame)
/// </summary>
public class OnRoadObjectSpawner : Spawner
{
    private OnRoadObjectMapGenerator objectMap;

    private int mapWidth = 3;

    private int mapLength = 30;

    private float difficulty = 0.25f;

    public override void StartSpawn()
    {
        objectMap = new OnRoadObjectMapGenerator(mapWidth, mapLength, difficulty, firstObjectSpawnPosition);
        base.StartSpawn();
    }

    public override void AddObject()
    {
        print("Вызов AddObject из OnRoadObjectSpawner");
        pooler.SpawnFromPool(poolTagToSpawnFrom.GetValue(), objectMap.GetPositionToSpawn());

        
    }

    //public override void AddObject()
    //{
    //    foreach (Cell cell in objectMap.GetMapGrid())
    //    {
    //        if (!cell.isEmpty)
    //        {
    //            pooler.SpawnFromPool(poolTagToSpawnFrom.GetValue(), cell.centerPosition);
    //            print("currentCell HashCode " + cell.GetType() + " - " + cell.GetHashCode());
    //        }
    //    }

    //    print("Вызов AddObject из OnRoadObjectSpawner");
    //}

}
