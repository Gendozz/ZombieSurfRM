using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRoadDelayedObjectGenerator : IObjectGenerator
{
    private readonly Vector3 firstObjectSpawnPosition;
    private readonly float betweenLaneDistance;
    private float currentZ;
    private float[] laneNumber = new float[3];
    private float minGenerationDistance = 2f;
    private float maxGenerationDistance = 10f;


    public OnRoadDelayedObjectGenerator(Vector3 firstObjectSpawnPosition, float betweenLaneDistance)
    {
        this.firstObjectSpawnPosition = firstObjectSpawnPosition;
        this.betweenLaneDistance = betweenLaneDistance;

        DefineLanes();
    }

    private void DefineLanes()
    {
        float currentX = firstObjectSpawnPosition.x;

        int counter = 0;
        do
        {
            laneNumber[counter] = currentX;
            currentX += betweenLaneDistance;
            counter++;
        }
        while (counter < 3);
    }

    public Vector3 GetPositionToSpawn()
    {
        float x = laneNumber[Random.Range(0, 3)];
        currentZ += Random.Range(minGenerationDistance, maxGenerationDistance);

        return new Vector3(x, firstObjectSpawnPosition.y, currentZ);
    }
}
