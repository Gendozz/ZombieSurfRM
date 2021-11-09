using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnersCreator : MonoBehaviour
{
    public ObjectPooler pooler;

    void Start()
    {

    }

    public void InstanciateSpawners()
    {
        pooler = ObjectPooler.Instance;
        List<ObjectPooler.Pool> pools = pooler.pools;
        for (int i = 0; i < pools.Count; i++)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
