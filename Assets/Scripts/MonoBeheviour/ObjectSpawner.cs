using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectSpawner : Spawner
{    
    //public int objectsAmountToInit = 10;

    //public StringReference poolTagToSpawnFrom;

    //[Tooltip("Position of the object first spawned from pool (start generating positing")]
    //public Vector3 firstObjectSpawnPosition;

    //private ObjectPooler pooler;

    //private GameObject lastSpawnedObject = null;

    //protected override void Init()
    //{
    //    print("Я в init ObjectSpawner");

    //    if (pooler == null)
    //    {
    //        Debug.LogError($"Spawner {gameObject.name} couldn't get object pooler");
    //        return;
    //    }
    //    lastSpawnedObject = pooler.SpawnFromPool(poolTagToSpawnFrom.GetValue(), firstObjectSpawnPosition);



    //    for (int i = 0; i < objectsAmountToInit - 1; i++)
    //    {
    //        AddObject();
    //    }
    //}

    //protected override void AddObject()
    //{
    //    Vector3 newPosition = lastSpawnedObject.GetComponent<ReplacableObject>().endPosition.position;
    //    lastSpawnedObject = pooler.SpawnFromPool(poolTagToSpawnFrom.GetValue(), newPosition);
    //}

    protected override void ReplaceObjectOutOfSee()             // TEMP SOLUTION
    {
        AddObject();
    }
}
