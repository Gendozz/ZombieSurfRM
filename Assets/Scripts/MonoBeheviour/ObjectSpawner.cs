using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectSpawner : MonoBehaviour
{    
    public int objectsAmountToInit = 10;

    public StringReference poolTagToSpawnFrom;

    [Tooltip("Position of the object first spawned from pool (start generating positing")]
    public Vector3 firstObjectSpawnPosition;

    private ObjectPooler pooler;

    private GameObject lastSpawnedObject = null;

    public void StartSpawn()
    {
        pooler = ObjectPooler.Instance;
        Init();
    }

    private void Init()
    {
        lastSpawnedObject = pooler.SpawnFromPool(poolTagToSpawnFrom.GetValue(), firstObjectSpawnPosition);

        for (int i = 0; i < objectsAmountToInit - 1; i++)
        {
            AddObject();
        }
    }

    public void AddObject()
    {
        Vector3 positionNew = lastSpawnedObject.GetComponent<ReplacableObject>().endPosition.position;
        lastSpawnedObject = pooler.SpawnFromPool(poolTagToSpawnFrom.GetValue(), positionNew);
    }

    public void ReplaceObjectOutOfSee()             // TEMP SOLUTION
    {
        AddObject();
    }
}
