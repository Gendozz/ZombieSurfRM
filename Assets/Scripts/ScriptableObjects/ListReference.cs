using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ListReference", menuName = "ListReference")]

public class ListReference : ScriptableObject
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
