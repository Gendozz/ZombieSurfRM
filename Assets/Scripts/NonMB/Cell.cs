using UnityEngine;

public struct Cell
{
    public Vector3 CenterPosition { get; private set; }

    public bool isEmpty;

    public Cell(float xPos, float zPos, bool isEmpty = false) 
    {
        CenterPosition = new Vector3(xPos, 0, zPos);
        this.isEmpty = isEmpty;
    }

}
