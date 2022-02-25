using System.Collections.Generic;
using UnityEngine;

public class OnRoadDelayedObjectGenerator : IObjectGenerator
{
    private float firstX;
    private float Ypos;
    private readonly float betweenLaneDistance;
    private float currentZ;
    private float[] xPositions = new float[3];
    private float minGenerationDistance = 2f;
    private float maxGenerationDistance = 10f;
    private FloatReference difficulty;

    private Queue<float> threeInRow = new Queue<float>(); // added


    public OnRoadDelayedObjectGenerator(float betweenLaneDistance, FloatReference difficulty, Vector3 firstObjectSpawnPosition)
    {
        firstX = firstObjectSpawnPosition.x;
        currentZ = firstObjectSpawnPosition.z;
        Ypos = firstObjectSpawnPosition.y;
        this.betweenLaneDistance = betweenLaneDistance;
        this.difficulty = difficulty;

        DefineLanes();
    }

    private void DefineLanes()
    {
        float currentX = firstX;

        int index = 0;
        do
        {
            xPositions[index] = currentX;
            currentX += betweenLaneDistance;
            index++;
            threeInRow.Enqueue(currentX); // added
        }
        while (index < 3);
    }

    public Vector3 GetPositionToSpawn()
    {
        float x = xPositions[Random.Range(0, 3)];
        float currentMaxGenerationDistance = maxGenerationDistance - difficulty.GetValue();
        if (currentMaxGenerationDistance < minGenerationDistance)
        {
            currentMaxGenerationDistance = minGenerationDistance;
        }
        currentZ += Random.Range(minGenerationDistance, currentMaxGenerationDistance);

        return new Vector3(x, Ypos, currentZ);
    }
}
