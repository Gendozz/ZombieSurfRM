using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameObjectReference", menuName = "GameObjectReference")]
public class GameObjectReference : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string Description = "";
#endif
    public GameObject value;

    public GameObject GetValue()
    {
        return value;
    }
}
