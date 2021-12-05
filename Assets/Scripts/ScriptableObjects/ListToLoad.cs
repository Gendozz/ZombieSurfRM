using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ListToLoad", menuName = "ListToLoad")]

public class ListToLoad : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline(5)]
    public string Description = "";
#endif

    public List<GameObject> listToLoad = new List<GameObject>();

    public List<GameObject> GetList()
    {
        return listToLoad;
    }
}
