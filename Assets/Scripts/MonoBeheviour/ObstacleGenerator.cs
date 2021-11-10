using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    private CellFrame currentCellFrame;

    public GameObject[] obstacleTypes;

    private int cellFrameWidth = 3;
    private int cellFrameLenght = 50;

    private float cellWidth = 2f;
    private float cellLenght = 2f;

    private int currentCellFrameStartZ = 0;

    [SerializeField]
    private FloatReference difficulty;

    // нужны логические переменные для определения наличия "стен" сверху, слева и справа => пока не понятно для чего

    float offset = 5f; // to use in future

    private void Start()
    {        
        // Инициация карты 
        currentCellFrame = new CellFrame(cellFrameWidth, cellFrameLenght);

        print($"cellFrameWidth => {cellFrameWidth} | cellFrameLenght => {cellFrameLenght}");

        FillCellFrameWithCells();

    }


    private void Update()                           // УДАЛИТЬ
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RefillCellFrame();
        }
    }


    /// <summary>
    /// Заполняет первую карту препятствий рандомными препятствиями
    /// </summary>
    public void FillCellFrameWithCells()           
    {
        // Заполняем CellFrame ячейками, устанавливая их координаты
        for (int width_X = 0; width_X < cellFrameWidth; width_X++)
        {
            float xPos = width_X * cellWidth + cellWidth / 2;

            for (int lenght_Z = 0; lenght_Z < cellFrameLenght; lenght_Z++)
            {
                float zPos = currentCellFrameStartZ + (lenght_Z * cellLenght + cellLenght / 2);

                Cell currentTile = new Cell(xPos, zPos);

                if (currentCellFrameStartZ == 0 || (currentCellFrameStartZ != 0 && lenght_Z != 0))  // не должно срабатывать при регенерации CellFrame, чтобы не закрывать проход.
                {
                    currentTile.isEmpty = Random.value > difficulty.GetValue();     // Прорежаем в зависимости от сложности 
                }
                currentCellFrame.cells[width_X, lenght_Z] = currentTile;
            }
        }

        MakeFreePath();
        FillCellFrame();
    }

    /// <summary>
    /// 1. "Прорубает" тропинку от первой линии тайлов до последней
    /// 2. Дополнительно прореживает препятствия в зависимости от сложности
    /// </summary>
    private void MakeFreePath()
    {
        // Выбираем рандомный тайл в первом ряду..
        int currentX = Random.Range(0, cellFrameWidth);
        int currentZ = 0;

        do
        {
            // .. делаем его пустым,
            // а по ходу цикла каждый впредеди идущий тайл, чтобы гарантировать всегда открытый путь
            currentCellFrame.cells[currentX, currentZ].isEmpty = true;

            // Выбираем направление для следующего "вырезания тайла"
            int sideCutDirection;

            // Если самый левый режем вправо
            if(currentX == 0) 
            {
                sideCutDirection = 1;
            }
            // Если самый правый режем влево
            else if(currentX == cellFrameWidth - 1)
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
        while (currentZ < cellFrameLenght);
    }


    /// <summary>
    /// Заполняет НЕпустые тайлы префабами
    /// Тест для отдельного использования
    /// </summary>
    private void FillCellFrame()
    {
        Cell currentCell;

        for (int width = 0; width < cellFrameWidth; width++)
        {
            for (int lenght = 0; lenght < cellFrameLenght; lenght++)
            {
                currentCell = currentCellFrame.cells[width, lenght];
                if (!currentCell.isEmpty)
                {
                    int randomObstacleIndex = Random.Range(0, obstacleTypes.Length);

                    Vector3 testCenterPosition = currentCell.centerPosition;
                    testCenterPosition.y += 2;
                    Instantiate(obstacleTypes[randomObstacleIndex], testCenterPosition, Quaternion.identity);
                }

            }
        }
    }

    public void RefillCellFrame()
    {
        CellFrame newCellFrame = new CellFrame(cellFrameWidth, cellFrameLenght);

        for (int width_X = 0; width_X < cellFrameWidth; width_X++)
        {
            newCellFrame.cells[width_X, 0] = currentCellFrame.cells[width_X, cellFrameLenght - 1];
        }        

        currentCellFrame = newCellFrame;

        currentCellFrameStartZ += cellFrameLenght * (int)cellLenght;

        FillCellFrameWithCells();

    }

    // Для тестов | удалить
    private void ShowCurrentCellFrameInConsole()
    {
        string line = "";
        for (int i = 0; i < cellFrameLenght; i++)
        {
            line = "";
            for (int j = 0; j < cellFrameWidth; j++)
            {
                line += " " + currentCellFrame.cells[j, i].isEmpty;
            }

            print(line);
        }
    }

}
