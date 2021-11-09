using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Двигает объекты в списке с заданной скоростью
/// </summary>
public class WorldMover : MonoBehaviour
{
    public FloatReference moveSpeed;

    private List<Transform> objectsToMove = new List<Transform>();

    private ObjectPooler pooler;

    private bool canMove = false;

    // Перед началом движения добавляем все контейнеры пулов в список объектов, которые будем двигать
    public void PrepareToMove()
    {
        pooler = ObjectPooler.Instance;

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
            obj.transform.position -= Vector3.forward * moveSpeed.value * Time.deltaTime; 
        }
    }

}
