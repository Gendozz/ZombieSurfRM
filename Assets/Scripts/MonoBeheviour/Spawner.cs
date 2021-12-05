﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class of all types of spawners.
/// Works with ObjectPooler.
/// </summary>
public class Spawner : MonoBehaviour
{
    [SerializeField]
    protected int objectsAmountToInit = 10;  //!!!

    public StringReference poolTagToSpawnFrom;

    [Tooltip("Position of the object first spawned from pool (start generating positing")]
    public Vector3 firstObjectSpawnPosition;

    protected ObjectPooler pooler;

    protected GameObject lastSpawnedObject = null;

    /// <summary>
    /// Gets pooler shared instance and calls Init()
    /// </summary>
    public virtual void StartSpawn()
    {
        pooler = ObjectPooler.SharedInstance;
        if(pooler == null)
        {
            Debug.LogError($"Spawner {gameObject.name} couldn't get object pooler");
            return;
        }

        Init();
    }

    /// <summary>
    /// Puts first few objects on scene
    /// </summary>
    protected virtual void Init()
    {
        lastSpawnedObject = pooler.SpawnFromPool(poolTagToSpawnFrom.GetValue(), firstObjectSpawnPosition);

        for (int i = 0; i < objectsAmountToInit - 1; i++)
        {
            AddObject();
        }
    }

    public virtual void AddObject() { }

    protected virtual void ReplaceObjectOutOfSee()             
    {
        AddObject();
    }
}
