using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid
{
    public Cell[,] cells;
    
    public MapGrid (int width, int lenght)
    {
        cells = new Cell[width, lenght];
    }
}
