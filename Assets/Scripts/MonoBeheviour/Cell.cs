using UnityEngine;

public struct Cell
{
    public Vector3 centerPosition { get; private set; }

    public bool isEmpty;

    public Cell(float xPos, float zPos, bool isEmpty = false) 
    {
        centerPosition = new Vector3(xPos, 2, zPos);
        this.isEmpty = isEmpty;
    }

}
