using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRoadObjectMapGenerator : IEnumerator<Cell>
{
    private MapGrid currentMapGrid;
    private int mapGridLanesAmount;
    private int mapGridRowsAmount;
    private float cellWidth = 3f;
    private float cellLenght = 2f;

    private Cell currentCell;

    private Vector3 firstObjectSpawnPosition;

    private int currentMapGridStartZ = 0;

    private float difficulty;

    private Vector3 endPoint;
    
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
    public OnRoadObjectMapGenerator(int mapGridLanesAmount, int mapGridRowsAmount, float difficulty, Vector3 firstObjectSpawnPosition)
    {
        this.mapGridLanesAmount = mapGridLanesAmount;
        this.mapGridRowsAmount = mapGridRowsAmount;
        this.difficulty = difficulty;
        this.firstObjectSpawnPosition = firstObjectSpawnPosition;        
        Reset();
    }

    /// <summary>
    /// Make changes to currentMapGrid so it's Cells got coordinates and final isEmpty property
    /// </summary>
    //private void MakeReadyToUseObjectMap()
    //{
    //    enumerator = GetGridEnumerable.GetEnumerator();
    //    currentMapGrid = new MapGrid(mapGridLanesAmount, mapGridRowsAmount);
    //    FillMapGridWithRandomIsEmptyCells();
    //    MakeIsEmptyPath();
    //}

    /// <summary>
    /// Initiates all Cells of currentMapGrid with coordinates and random isEmpty property
    /// </summary>
    public void FillMapGridWithRandomIsEmptyCells()
    {
        // Заполняем CellFrame ячейками, устанавливая их координаты
        for (int laneNumber = 0; laneNumber < mapGridLanesAmount; laneNumber++)
        {
            float xPos = laneNumber * cellWidth + cellWidth / 2 + firstObjectSpawnPosition.x;

            for (int rowNumber = 0; rowNumber < mapGridRowsAmount; rowNumber++)
            {
                float zPos = currentMapGridStartZ + (rowNumber * cellLenght + cellLenght / 2);

                if (laneNumber == 0 && rowNumber == 0)
                {
                    zPos = GetFirstCellPosition().z;
                }

                currentCell = new Cell(xPos, zPos);

                if (currentMapGridStartZ == 0 || (currentMapGridStartZ != 0 && rowNumber != 0))  // не должно срабатывать при регенерации CellFrame, чтобы не закрывать проход.
                {
                    currentCell.isEmpty = Random.value > difficulty;     // Прорежаем в зависимости от сложности 
                }
                currentMapGrid.cells[laneNumber, rowNumber] = currentCell;
            }
        }
    }

    /// <summary>
    /// 1. Make "isEmpty" path from first row (x = 0) of mapGrid to last row (x = mapGrid.Length - 1) 
    /// </summary>
    private void MakeIsEmptyPath()
    {
        // Выбираем рандомный тайл в первом ряду..
        int currentX = Random.Range(0, mapGridLanesAmount);
        int currentZ = 0;

        do
        {
            // .. делаем его пустым,
            // а по ходу цикла каждый впредеди идущий тайл, чтобы гарантировать всегда открытый путь
            currentMapGrid.cells[currentX, currentZ].isEmpty = true;

            // Выбираем направление для следующего "вырезания тайла"
            int sideCutDirection;

            // Если самый левый режем вправо
            if (currentX == 0)
            {
                sideCutDirection = 1;
            }
            // Если самый правый режем влево
            else if (currentX == mapGridLanesAmount - 1)
            {
                sideCutDirection = -1;
            }
            // Если по середине выбираем рандомное направлением (влево, вправо)
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
        } while (!mapEnumerator.Current.isEmpty);


        Cell cell = mapEnumerator.Current;
        position = cell.centerPosition;

        return position;
        //IEnumerator<Cell> cellEnumerator = GetNextCellToSpawnInto().GetEnumerator();

        //if (cellEnumerator.MoveNext())
        //{
        //    Debug.Log("currentCell HashCode " + cellEnumerator.Current + " - " + cellEnumerator.Current.GetHashCode());
        //    return cellEnumerator.Current.centerPosition;
        //}

        //Debug.Log("No next cell <= FROM OnRoadObjectMapGenerator");
        //return Vector3.zero;
    }

    public IEnumerable<Cell> GetGridEnumerable()
    {
        foreach (Cell cell in currentMapGrid)
        {
            yield return cell;
        }
    }

    private Vector3 GetLeftEndPointCoords()
    {
        return currentMapGrid.cells[0, mapGridRowsAmount - 1].centerPosition;
    }

    private Vector3 GetFirstCellPosition()
    {
        float nextFirstCellZPosition = GetLeftEndPointCoords().z + cellLenght;

        return new Vector3(endPoint.x, endPoint.y, nextFirstCellZPosition);
    }

    public void RefillMapGrid()
    {
        MapGrid newMapGrid = new MapGrid(mapGridLanesAmount, mapGridRowsAmount);


        // первый ряд новой карты всегда равен последнему ряду текущей карты
        for (int width_X = 0; width_X < mapGridLanesAmount; width_X++)
        {
            newMapGrid.cells[width_X, 0] = currentMapGrid.cells[width_X, mapGridRowsAmount - 1];
        }        

        currentMapGrid = newMapGrid;

        currentMapGridStartZ += mapGridRowsAmount * (int)cellLenght;

        FillMapGridWithRandomIsEmptyCells();

    }

    #region For tests, delete before release
    private void ShowCurrentCellFrameInConsole()
    {
        string line = "";
        for (int i = 0; i < mapGridRowsAmount; i++)
        {
            line = "";
            for (int j = 0; j < mapGridLanesAmount; j++)
            {
                line += " " + currentMapGrid.cells[j, i].isEmpty;
            }

            Debug.Log(line);
        }
    }

    /// <summary>
    /// Заполняет НЕпустые тайлы префабами
    /// Тест для отдельного использования
    /// </summary>
    //private void FillObjectMapWithPrefabs()
    //{
    //    Cell localCell;

    //    for (int width = 0; width < mapGridWidth; width++)
    //    {
    //        for (int lenght = 0; lenght < mapGridLenght; lenght++)
    //        {
    //            localCell = currentMapGrid.cells[width, lenght];
    //            if (!localCell.isEmpty)
    //            {
    //                int randomObstacleIndex = Random.Range(0, obstacleTypes.Length);

    //                Vector3 testCenterPosition = localCell.centerPosition;
    //                testCenterPosition.y += 2;
    //                Instantiate(obstacleTypes[randomObstacleIndex], testCenterPosition, Quaternion.identity);
    //            }
    //        }
    //    }
    //}



    //private void Update()                           
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        RefillMapGrid();
    //        MakeIsEmptyPath();
    //        FillObjectMapWithPrefabs();
    //    }
    //} 
    #endregion

    public MapGrid GetMapGrid()
    {
        return currentMapGrid;
    }

    #region IEnumeratorImplementation
    public bool MoveNext()
    {
        return mapEnumerator.MoveNext();
    }

    public void Reset()
    {
        mapEnumerator = GetGridEnumerable().GetEnumerator();
        currentMapGrid = new MapGrid(mapGridLanesAmount, mapGridRowsAmount);
        FillMapGridWithRandomIsEmptyCells();
        MakeIsEmptyPath();
    }

    public void Dispose()
    {
        throw new System.NotImplementedException();
    } 
    #endregion
}
