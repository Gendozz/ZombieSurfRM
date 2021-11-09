using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cell
{
    public Vector3 centerPosition { get; set; }

    public bool isEmpty;

    public Cell(float xPos, float zPos, bool isEmpty = false) 
    {
        centerPosition = new Vector3(xPos, 0, zPos);
        this.isEmpty = isEmpty;
    }

}
