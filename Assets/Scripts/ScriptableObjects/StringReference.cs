using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StringReference", menuName = "StringReference")]
public class StringReference : ScriptableObject
{

#if UNITY_EDITOR
    [Multiline]
    public string Description = "";
#endif
    [SerializeField]
    private string value;

    public string GetValue()
    {
        return value;
    }
}
