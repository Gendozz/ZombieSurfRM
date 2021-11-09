using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadFiller : MonoBehaviour
{
    private TileMap currentMap;

    public GameObject[] obstacleTypes;

    private int mapWidth = 3;
    private int mapLenght = 50;

    private float tileWidth = 2f;
    private float tileLenght = 2f;

    private int currentMapStartZ = 0;

    [SerializeField]
    private FloatReference difficulty;

    // нужны логические переменные для определения наличия "стен" сверху, слева и справа => пока не понятно для чего

    float offset = 5f; // to use in future

    private void Start()
    {        
        // Инициация карты 
        currentMap = new TileMap(mapWidth, mapLenght);

        print($"mapWidth => {mapWidth} | mapLenght => {mapLenght}");

        FillTileMapWithTiles();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RefillMap();
        }
    }


    /// <summary>
    /// Заполняет первую карту препятствий рандомными препятствиями
    /// </summary>
    public void FillTileMapWithTiles()           
    {
        // Заполняем TileMap тайлами, устанавливая их координаты
        for (int width_X = 0; width_X < mapWidth; width_X++)
        {
            float xPos = width_X * tileWidth + tileWidth / 2;

            for (int lenght_Z = 0; lenght_Z < mapLenght; lenght_Z++)
            {
                float zPos = currentMapStartZ + (lenght_Z * tileLenght + tileLenght / 2);

                Tile currentTile = new Tile(xPos, zPos);

                if (currentMapStartZ == 0 || (currentMapStartZ != 0 && lenght_Z != 0))  // не должно срабатывать при регенерации tilemap, чтобы не закрывать проход.
                {
                    currentTile.isEmpty = Random.value > difficulty.GetValue();     // Прорежаем в зависимости от сложности 
                }
                currentMap.tiles[width_X, lenght_Z] = currentTile;
            }
        }

        MakeFreePath();
        FillMap();
    }

    /// <summary>
    /// 1. "Прорубает" тропинку от первой линии тайлов до последней
    /// 2. Дополнительно прореживает препятствия в зависимости от сложности
    /// </summary>
    private void MakeFreePath()
    {
        // Выбираем рандомный тайл в первом ряду..
        int currentX = Random.Range(0, mapWidth);
        int currentZ = 0;

        do
        {
            // .. делаем его пустым,
            // а по ходу цикла каждый впредеди идущий тайл, чтобы гарантировать всегда открытый путь
            currentMap.tiles[currentX, currentZ].isEmpty = true;

            // Выбираем направление для следующего "вырезания тайла"
            int sideCutDirection;

            // Если самый левый режем вправо
            if(currentX == 0) 
            {
                sideCutDirection = 1;
            }
            // Если самый правый режем влево
            else if(currentX == mapWidth - 1)
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
        while (currentZ < mapLenght);
    }

    private void FillMap()
    {
        Tile currentTile;

        for (int width = 0; width < mapWidth; width++)
        {
            for (int lenght = 0; lenght < mapLenght; lenght++)
            {
                currentTile = currentMap.tiles[width, lenght];
                if (!currentTile.isEmpty)
                {
                    int randomObstacleIndex = Random.Range(0, obstacleTypes.Length);

                    Vector3 testCenterPosition = currentTile.centerPosition;
                    testCenterPosition.y += 2;
                    Instantiate(obstacleTypes[randomObstacleIndex], testCenterPosition, Quaternion.identity);
                }

            }
        }
    }

    // Для тестов
    private void ShowCurrentMapInConsole()
    {
        string line = "";
        for (int i = 0; i < mapLenght; i++)
        {
            line = "";
            for (int j = 0; j < mapWidth; j++)
            {
                line += " " + currentMap.tiles[j, i].isEmpty;
            }

            print(line);
        }
    }

    public void RefillMap()
    {
        TileMap newMap = new TileMap(mapWidth, mapLenght);

        for (int width_X = 0; width_X < mapWidth; width_X++)
        {
            newMap.tiles[width_X, 0] = currentMap.tiles[width_X, mapLenght - 1];
        }        

        currentMap = newMap;

        currentMapStartZ += mapLenght * (int)tileLenght;

        FillTileMapWithTiles();

    }
}
