using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FloatReference", menuName = "FloatReference")]
public class FloatReference : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline(5)]
    public string Description = "";
#endif
    public float value;

    public float GetValue()
    {
        return value;
    }
}
