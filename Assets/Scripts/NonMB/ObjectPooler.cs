using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Dictonary-based object pooler
/// </summary>
public class ObjectPooler : MonoBehaviour
{
    /// <summary>
    /// Single pool template
    /// </summary>
    [System.Serializable]
    public class Pool
    {
        [HideInInspector]
        public string itemName;

        public StringReference poolTag;

        public GameObject[] prefabs;

        // Pool size
        public int size;

        // The container in which the pool prefabs are placed
        public Transform container;
    }

    public UnityEvent poolIsReady;

    // A la singlton
    public static ObjectPooler SharedInstance;

    private void Awake()
    {
        SharedInstance = this;
    }

    /// <summary>
    /// List of pools. Fill in inspector
    /// </summary>
    public List<Pool> pools;

    // Словарь пулов
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    /// <summary>
    /// Initialize the pool dictionary with prefabs, put each pool objects in their own container
    /// </summary>
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // Create container for each pool, fill it with perfabs ...
        foreach (Pool pool in pools)
        {
            pool.container = new GameObject("ContainerForPooled_" + pool.poolTag.GetValue() + "s").transform;

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefabs[Random.Range(0, pool.prefabs.Length)], pool.container);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            // ... and put it in dictionary with key == poolTag

            poolDictionary.Add(pool.poolTag.GetValue(), objectPool);
            //Debug.Log($"Пул {pool.poolTag.GetValue()} готов");                      // Debug
        }
        //Debug.Log("Все пулы готовы");                                               // Debug
        poolIsReady?.Invoke();
    }

    /// <summary>
    /// Returns object from the pool and puts it in the scene in specified position
    /// </summary>
    /// <param name="tag">Pool tag in which the object is taken</param>
    /// <param name="position">Position at which object will be placed</param>
    /// <returns></returns>
    public GameObject SpawnFromPool(string tag, Vector3 position, bool convertToLocalPosition)
    {
        // If there is no dictionary with given tag - do nothing
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"There's no pool with tag => {tag}");
            return null;
        }

        // Get object from pool, set it's 'active' to true, place object in the scene and put it back to the pool
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();


        objectToSpawn.transform.position = position;

        if (convertToLocalPosition)
        {
            objectToSpawn.transform.localPosition = objectToSpawn.transform.position;
        }
        objectToSpawn.SetActive(true);

        // Call OnObjectSpawn() on current getted from pool object
        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        // Put in back in pool in the end of queue
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    private void OnValidate()
    {
        foreach (Pool pool in pools)
        {
            string name = pool.poolTag.GetValue();
            pool.itemName = string.IsNullOrEmpty(name) ? "Empty" : name;
        }
    }
}
