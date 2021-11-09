using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Пул объектов на основе словаря
/// </summary>
public class ObjectPooler : MonoBehaviour
{
    /// <summary>
    /// Шаблон пула
    /// </summary>
    [System.Serializable]
    public class Pool
    {
        // Тэг пула
        public StringReference poolTag;

        // Префаб, помещаемый в пул
        public GameObject prefab;

        // Размер пула
        public int size;

        // Контейнер, в который помещаются префабы пула
        public Transform container;
    }


    public UnityEvent poolIsReady;

    // А-ля синглтон
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Список пулов
    public List<Pool> pools;
    
    // Словарь пулов
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    /// <summary>
    /// Инициализируем словарь пулов префабами, каждый пул помещаем в свой контенйер
    /// </summary>
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // Создаём контейнер для каждого пула из списка пулов, заполняем его префабами ...
        foreach (Pool pool in pools)
        {            
            pool.container = new GameObject("ContainerForPooled_" + pool.poolTag.GetValue() + "s").transform;

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, pool.container); 
                obj.SetActive(false);
                objectPool.Enqueue(obj);                
            }
            // ... и помещаем в словарь с ключём == тэгу пула
         
            poolDictionary.Add(pool.poolTag.GetValue(), objectPool);
            print($"Пул {pool.poolTag.GetValue()} готов");                     // Debug
        }  
        print("Все пулы готовы");                               // Debug
        poolIsReady?.Invoke();        
    }

    /// <summary>
    /// Возвращает объект из пула и размещает его на сцене в указанной позиции
    /// </summary>
    /// <param name="tag">Тэг пула, из которого берётся объект</param>
    /// <param name="position">Координаты, в которых размещается объект</param>
    /// <returns></returns>
    public GameObject SpawnFromPool(string tag, Vector3 position)
    {
        // Если словаря с полученным tag нет - ничего не делаем
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"There's no pool with tag => {tag}");
            return null;
        }

        // Достаём из пула объект, активируем, размещаем его на сцене и кладём обратно в пул
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.transform.position = position;

        objectToSpawn.SetActive(true);

        // Для чего я это делал?))
        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }
        // не понятно

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    /// <summary>
    /// Предоставляет ссылку на объект, который будет возвращён следующим из пула по тэгу "listTagToPeek"
    /// </summary>
    /// <param name="listTagToPeek"></param>
    /// <returns></returns>
    public GameObject GetObjectToReplace(string listTagToPeek)
    {
        if (!poolDictionary.ContainsKey(listTagToPeek))
        {
            Debug.Log($"There's no pool with tag => {listTagToPeek}");
            return null;
        }
        return poolDictionary[listTagToPeek].Peek();
    }
}
