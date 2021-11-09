using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Создаёт все объекты, необходимые для старта игры
/// </summary>
public class LevelLoader : MonoBehaviour
{
    public ListToLoad listToLoad;

    void Start()
    {
        GameObject container = new GameObject("Spawners_pools_etc");
        foreach (var obj in listToLoad.GetList())
        {
            Instantiate(obj, container.transform);
        }
    }


}
