using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    protected int objectsAmountToInit = 10;  //!!!

    public StringReference poolTagToSpawnFrom;

    [Tooltip("Position of the object first spawned from pool (start generating positing")]
    public Vector3 firstObjectSpawnPosition;

    protected ObjectPooler pooler;

    protected GameObject lastSpawnedObject = null;

    public void StartSpawn()
    {
        pooler = ObjectPooler.SharedInstance;
        if(pooler == null)
        {
            Debug.LogError($"Spawner {gameObject.name} couldn't get object pooler");
            return;
        }

        Init();
    }

    protected virtual void Init()
    {
        print("Я в init Spawner");

        lastSpawnedObject = pooler.SpawnFromPool(poolTagToSpawnFrom.GetValue(), firstObjectSpawnPosition);

        for (int i = 0; i < objectsAmountToInit - 1; i++)
        {
            AddObject();
        }
    }

    protected virtual void AddObject()
    {
        Vector3 newPosition = lastSpawnedObject.GetComponent<ReplacableObject>().endPosition.position;
        lastSpawnedObject = pooler.SpawnFromPool(poolTagToSpawnFrom.GetValue(), newPosition);
    }

    protected virtual void ReplaceObjectOutOfSee()             // TEMP SOLUTION
    {
        AddObject();
    }
}
