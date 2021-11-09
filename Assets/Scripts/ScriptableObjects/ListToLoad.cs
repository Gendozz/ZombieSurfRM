using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ListToLoad", menuName = "ListToLoad")]

public class ListToLoad : ScriptableObject
{
    public List<GameObject> listToLoad = new List<GameObject>();

    public List<GameObject> GetList()
    {
        return listToLoad;
    }
}
