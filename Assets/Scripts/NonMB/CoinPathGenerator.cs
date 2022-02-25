using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPathGenerator : IObjectGenerator
{
    // Positions
    private readonly Vector3 firstObjectSpawnPosition;
    private float yPos;
    private float currentZ;
    private float previousX;
    private int xPositionIndex;
    private float[] xPositions = new float[3];

    // Distances
    private readonly float betweenLaneDistance;
    private readonly float inPathGenerationDistance = 2f;
    private readonly float maxDistanceBetweenPaths = 60f;
    private readonly float minDistanceBetweenPaths = 30f;

    // Amounts
    private readonly int minAmountInOnePath = 10;
    private readonly int maxAmountInOnePath = 20;
    private int amountInCurrentPath;
    private readonly int minAmountInline = 3;
    private readonly int maxAmountInline = 10;
    private int amountInCurrentLine;
    private int inPathCounter = 20;
    private int inlineCounter = 0;

    // Other

    // From 0 to 10. The more is - the less chance to change lane randomly
    private int changeLaneRandomnesIndex = 6;

    public CoinPathGenerator(float betweenLaneDistance, Vector3 firstObjectSpawnPosition)
    {
        this.firstObjectSpawnPosition = firstObjectSpawnPosition;
        yPos = firstObjectSpawnPosition.y;
        this.betweenLaneDistance = betweenLaneDistance;
        previousX = firstObjectSpawnPosition.x;

        DefineLanes();
    }

    private void DefineLanes()
    {
        float currentX = firstObjectSpawnPosition.x;

        int index = 0;
        do
        {
            xPositions[index] = currentX;
            currentX += betweenLaneDistance;
            index++;
        }
        while (index < 3);
    }

    public Vector3 GetPositionToSpawn()
    {
        float currentX;

        if (IsNextPath())
        {
            // Setting random amount of coins in currentpath and line, setting X and Z positions, resetting counters
            amountInCurrentPath = Random.Range(minAmountInOnePath, maxAmountInOnePath);
            amountInCurrentLine = Random.Range(minAmountInline, maxAmountInline);
            currentZ += Random.Range(minDistanceBetweenPaths, maxDistanceBetweenPaths);
            xPositionIndex = Random.Range(0, xPositions.Length);
            currentX = xPositions[xPositionIndex];
            inPathCounter = 0;
            inlineCounter = 0;
        }
        else
        {
            currentZ += inPathGenerationDistance;

            bool isSureToChangeLane = false;

            if (inlineCounter >= minAmountInline)
            {
                if (inlineCounter >= amountInCurrentLine)
                {
                    isSureToChangeLane = true;
                }
                else
                {
                    isSureToChangeLane = Random.Range(0, 10) > changeLaneRandomnesIndex;
                }
            }

            if (isSureToChangeLane)
            {
                amountInCurrentLine = Random.Range(minAmountInline, maxAmountInline);
                xPositionIndex = GetNextLaneNumber();
                currentX = xPositions[xPositionIndex];
                inlineCounter = 0;
            }
            else
            {
                currentX = xPositions[xPositionIndex];
            }
        }

        inPathCounter++;
        inlineCounter++;

        return new Vector3(currentX, yPos, currentZ);
    }

    private bool IsNextPath()
    {
        if (inPathCounter >= amountInCurrentPath)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Change lane of generated coin
    /// </summary>
    private int GetNextLaneNumber()
    {
        if (xPositionIndex == 0 || xPositionIndex == 2)
        {
            return 1;
        }
        else
        {
            bool toLeft = Random.Range(1, 10) > 5;
            if (toLeft)
            {
                return 0;
            }
            else
            {
                return 2;
            }
        }
    }
}
