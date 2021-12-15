﻿using UnityEngine;
using UnityEngine.Events;

public class ReplacableObject : MonoBehaviour, IPooledObject
{
    public Transform endPosition;

    public UnityEvent objectIsOutOfSee;

    public FloatReference minZ;

    public bool isSpawned = false;

    public void OnObjectSpawn()
    {
        isSpawned = true;
    }

    private void Update()
    {
        if(isSpawned && transform.position.z < minZ.value)
        {
            print($"Object with name {gameObject.name} invoked |objectIsOutOfSee| Event");
            objectIsOutOfSee?.Invoke();
            isSpawned = false;
            gameObject.SetActive(false);
        }
    }
}
