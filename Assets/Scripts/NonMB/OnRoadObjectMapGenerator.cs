using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRoadObjectMapGenerator : IEnumerator<Cell>, IObjectGenerator
{
    private MapGrid currentMapGrid;
    private int mapGridLanesAmount;
    private int mapGridRowsAmount;
    private float cellWidth = 3f;
    private float cellLenght = 4f;

    private Cell currentCell;

    private readonly Vector3 firstObjectSpawnPosition;

    private float currentMapGridStartZ = 0;

    private FloatReference difficulty;
    
    private IEnumerator<Cell> mapEnumerator;

    public Cell Current
    {
        get
        {
            return mapEnumerator.Current;
        }
    }

    object IEnumerator.Current => throw new System.NotImplementedException();

    #region OnStartSpawnMethods
    public OnRoadObjectMapGenerator(int mapGridLanesAmount, int mapGridRowsAmount, FloatReference difficulty, Vector3 firstObjectSpawnPosition)
    {
        this.mapGridLanesAmount = mapGridLanesAmount;
        this.mapGridRowsAmount = mapGridRowsAmount;
        this.difficulty = difficulty;
        this.firstObjectSpawnPosition = firstObjectSpawnPosition;
        MakeReadyToUseObjectMap();
    }

    /// <summary>
    /// Make changes to currentMapGrid so it's Cells got coordinates and isEmpty property.
    /// It's shell around Reset() because Reset() was needed to implement IEnumerator and
    /// both of them implemented the same functionality
    /// </summary>
    private void MakeReadyToUseObjectMap()
    {
        Reset();
    }

    /// <summary>
    /// Initiates all Cells of currentMapGrid with coordinates and random isEmpty property
    /// </summary>
    private void FillMapGridWithRandomEmptyCells()
    {
        // Fill CellFrame with cells, setting their coords
        for (int laneNumber = 0; laneNumber < mapGridLanesAmount; laneNumber++)         
        {
            float xPos = laneNumber * cellWidth + firstObjectSpawnPosition.x;           

            for (int rowNumber = 0; rowNumber < mapGridRowsAmount; rowNumber++)         
            {
                float zPos = currentMapGridStartZ + (rowNumber * cellLenght) + firstObjectSpawnPosition.z;

                currentCell = new Cell(xPos, zPos);

                if (currentMapGridStartZ == 0 || (currentMapGridStartZ != 0 && rowNumber != 0))  // should not be triggered when CellFrame is regenerated so as not to block the path
                {
                    currentCell.isEmpty = Random.value > difficulty.GetValue();     // thin depending on the difficulty
                }
                currentMapGrid.cells[laneNumber, rowNumber] = currentCell;
            }
        }
        currentMapGridStartZ = currentMapGrid.cells[0, mapGridRowsAmount - 1].CenterPosition.z + cellLenght;

    }

    /// <summary>
    /// 1. Make "isEmpty" path from first row (x = 0) of mapGrid to last row (x = mapGrid.Length - 1) 
    /// </summary>
    private void MakeIsEmptyPath()
    {
        // Choose random cell in the first row..
        int currentX = Random.Range(0, mapGridLanesAmount);
        int currentZ = 0;

        do
        {
            // .. make it empty,
            // and during the cycle, each cell that goes ahead to ensure that the path is always open
            currentMapGrid.cells[currentX, currentZ].isEmpty = true;

            // Choose the direction for the next "cell cutting"
            int sideCutDirection;

            // If leftmost - cut to right
            if (currentX == 0)
            {
                sideCutDirection = 1;
            }
            // If rightmost - cut to left
            else if (currentX == mapGridLanesAmount - 1)
            {
                sideCutDirection = -1;
            }
            // is at the middle choose random (left or right)
            else
            {
                float sign = Mathf.Sign((float)Random.Range(-1f, 1f));
                sideCutDirection = (int)sign;
            }

            if (Random.value > 0.5f)
            {
                currentX += sideCutDirection;
            }
            else
            {
                currentZ++;
            }
        }
        while (currentZ < mapGridRowsAmount);
    }
    #endregion

    /// <summary>
    /// Return position to spawn next object
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPositionToSpawn()
    {
        Vector3 position;

        do
        {
            if (!MoveNext())
            {
                Reset();
                MoveNext();
            } 
        } while (mapEnumerator.Current.isEmpty);

        Cell cell = mapEnumerator.Current;
        position = cell.CenterPosition;

        return position;
    }

    /// <summary>
    /// Just for iterate trough mapGrid
    /// </summary>
    /// <returns></returns>
    private IEnumerable<Cell> GetGridEnumerable()
    {
        foreach (Cell cell in currentMapGrid)
        {
            yield return cell;
        }
    }


    #region IEnumerator<> Implementation
    public bool MoveNext()
    {
        return mapEnumerator.MoveNext();
    }

    public void Reset()
    {        
        currentMapGrid = new MapGrid(mapGridLanesAmount, mapGridRowsAmount);
        mapEnumerator = GetGridEnumerable().GetEnumerator();    
        FillMapGridWithRandomEmptyCells();
        MakeIsEmptyPath();
    }

    public void Dispose()
    {
        throw new System.NotImplementedException();
    } 
    #endregion
}
