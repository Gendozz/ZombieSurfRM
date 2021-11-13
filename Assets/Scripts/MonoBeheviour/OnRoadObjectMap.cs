using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRoadObjectMap : MonoBehaviour
{
    private MapGrid currentMapGrid;

    private Cell currentCell;

    public GameObject[] obstacleTypes;

    private int mapGridWidth = 3;
    private int mapGridLenght = 50;

    private float cellWidth = 2f;
    private float cellLenght = 2f;

    private int currentMapGridStartZ = 0;

    [SerializeField]
    private FloatReference difficulty;

    // нужны логические переменные для определения наличия "стен" сверху, слева и справа => пока не понятно для чего

    float offset = 5f; // to use in future

    private void Start()
    {        
        currentMapGrid = new MapGrid(mapGridWidth, mapGridLenght);

        MakeReadyToUseObjectMap();
        print("End of Start");

    }

    /// <summary>
    /// Make changes to currentMapGrid so it's Cells got coordinates and final isEmpty property
    /// </summary>
    private void MakeReadyToUseObjectMap()
    {
        FillMapGridWithRandomIsEmptyCells();
        MakeIsEmptyPath();
        FillObjectMapWithPrefabs();

    }

    /// <summary>
    /// Initiates all Cells of currentMapGrid with coordinates and randomly isEmpty property
    /// </summary>
    public void FillMapGridWithRandomIsEmptyCells()           
    {
        // Заполняем CellFrame ячейками, устанавливая их координаты
        for (int width_X = 0; width_X < mapGridWidth; width_X++)
        {
            float xPos = width_X * cellWidth + cellWidth / 2;

            for (int lenght_Z = 0; lenght_Z < mapGridLenght; lenght_Z++)
            {
                float zPos = currentMapGridStartZ + (lenght_Z * cellLenght + cellLenght / 2);

                currentCell = new Cell(xPos, zPos);

                if (currentMapGridStartZ == 0 || (currentMapGridStartZ != 0 && lenght_Z != 0))  // не должно срабатывать при регенерации CellFrame, чтобы не закрывать проход.
                {
                    currentCell.isEmpty = Random.value > difficulty.GetValue();     // Прорежаем в зависимости от сложности 
                }
                currentMapGrid.cells[width_X, lenght_Z] = currentCell;
            }
        }
    }


    /// <summary>
    /// 1. Make "isEmpty" path from first row (x = 0) of mapGrid to last row (x = mapGrid.Length - 1) 
    /// </summary>
    private void MakeIsEmptyPath()
    {
        // Выбираем рандомный тайл в первом ряду..
        int currentX = Random.Range(0, mapGridWidth);
        int currentZ = 0;

        do
        {
            // .. делаем его пустым,
            // а по ходу цикла каждый впредеди идущий тайл, чтобы гарантировать всегда открытый путь
            currentMapGrid.cells[currentX, currentZ].isEmpty = true;

            // Выбираем направление для следующего "вырезания тайла"
            int sideCutDirection;

            // Если самый левый режем вправо
            if(currentX == 0) 
            {
                sideCutDirection = 1;
            }
            // Если самый правый режем влево
            else if(currentX == mapGridWidth - 1)
            {
                sideCutDirection = -1;
            }
            // Если по середине выбираем рандомное направлением (влево, вправо)
            else
            {
                float sign = Mathf.Sign((float)Random.Range(-1f, 1f));
                sideCutDirection = (int)sign ;
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
        while (currentZ < mapGridLenght);
    }


    /// <summary>
    /// Заполняет НЕпустые тайлы префабами
    /// Тест для отдельного использования
    /// </summary>
    private void FillObjectMapWithPrefabs()
    {
        Cell localCell;

        for (int width = 0; width < mapGridWidth; width++)
        {
            for (int lenght = 0; lenght < mapGridLenght; lenght++)
            {
                localCell = currentMapGrid.cells[width, lenght];
                if (!localCell.isEmpty)
                {
                    int randomObstacleIndex = Random.Range(0, obstacleTypes.Length);

                    Vector3 testCenterPosition = localCell.centerPosition;
                    testCenterPosition.y += 2;
                    Instantiate(obstacleTypes[randomObstacleIndex], testCenterPosition, Quaternion.identity);
                }
            }
        }
    }

    public void RefillMapGrid()
    {
        MapGrid newMapGrid = new MapGrid(mapGridWidth, mapGridLenght);


        // первый ряд новой карты всегда равен последнему ряду текущей карты
        for (int width_X = 0; width_X < mapGridWidth; width_X++)
        {
            newMapGrid.cells[width_X, 0] = currentMapGrid.cells[width_X, mapGridLenght - 1];
        }        

        currentMapGrid = newMapGrid;

        currentMapGridStartZ += mapGridLenght * (int)cellLenght;

        FillMapGridWithRandomIsEmptyCells();

    }

    #region For tests, delete before release
    private void ShowCurrentCellFrameInConsole()
    {
        string line = "";
        for (int i = 0; i < mapGridLenght; i++)
        {
            line = "";
            for (int j = 0; j < mapGridWidth; j++)
            {
                line += " " + currentMapGrid.cells[j, i].isEmpty;
            }

            print(line);
        }
    }

    private void Update()                           
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Space pressed");
            RefillMapGrid();
            MakeIsEmptyPath();
            FillObjectMapWithPrefabs();
        }
    } 
    #endregion

}
