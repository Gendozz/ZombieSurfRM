using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tile
{
    public Vector3 centerPosition { get; set; }

    public bool isEmpty;

    public Tile(float xPos, float zPos, bool isEmpty = false) 
    {
        centerPosition = new Vector3(xPos, 0, zPos);
        this.isEmpty = isEmpty;
    }

}
