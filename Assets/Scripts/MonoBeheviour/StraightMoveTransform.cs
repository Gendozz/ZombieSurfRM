using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMoveTransform : MonoBehaviour
{
    private float speed = 10;

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
