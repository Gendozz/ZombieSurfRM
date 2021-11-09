using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap
{
    public Tile[,] tiles;
    
    public TileMap (int width, int lenght)
    {
        tiles = new Tile[width, lenght];
    }
}
