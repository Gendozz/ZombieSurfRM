using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFrame
{
    public Cell[,] cells;
    
    public CellFrame (int width, int lenght)
    {
        cells = new Cell[width, lenght];
    }
}
