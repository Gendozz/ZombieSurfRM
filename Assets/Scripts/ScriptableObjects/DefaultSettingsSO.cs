using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DefaultSettings", menuName = "DefualtSettings")]
public class DefaultSettingsSO : ScriptableObject
{
    [SerializeField]
    private float defaultDifficulty;

    public float GetDefaultDifficulty()
    {
        return defaultDifficulty;
    }
}
