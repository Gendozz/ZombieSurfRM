using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private ObstacleType obstacleType;

    public enum ObstacleType
    {
        LOW,
        HIGH,
        HIGH_SOLID
    }

    public ObstacleType GetType()
    {
        return obstacleType;
    }
}
