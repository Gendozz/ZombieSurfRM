using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Vector3Reference", menuName = "Vector3Reference")]
public class Vector3Reference : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string Description = "";
#endif

    public Vector3 value; 
}
