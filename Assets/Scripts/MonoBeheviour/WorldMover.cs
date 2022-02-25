using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Move objects at a given speed
/// </summary>
public class WorldMover : MonoBehaviour
{
    [SerializeField]
    private FloatReference moveSpeed;

    [SerializeField]
    private FloatReference difficulty;

    private List<Transform> objectsToMove = new List<Transform>();

    private ObjectPooler pooler;

    private bool canMove = false;

    /// <summary>
    /// Add all pool containers which will be moving
    /// </summary>
    public void PrepareToMove()
    {
        pooler = ObjectPooler.SharedInstance;

        for (int i = 0; i < pooler.pools.Count; i++)
        {
            objectsToMove.Add(pooler.pools[i].container);
        }
        canMove = true;
    }

    private void Update()
    {
        if (!canMove) 
            return;

        foreach (var obj in objectsToMove)
        {
            obj.transform.position -= Vector3.forward * moveSpeed.GetValue() * (1 + difficulty.GetValue()) * Time.deltaTime; 
        }
    }

    public void StopMove()
    {
        canMove = false;
    }

}
