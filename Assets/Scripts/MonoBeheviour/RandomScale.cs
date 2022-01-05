using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScale : MonoBehaviour
{
    private float minScale = 0.5f;
    private float maxScale = 2.5f;

    void Start()
    {
        transform.localScale = transform.localScale + Vector3.one * Random.Range(minScale, maxScale);
    }

}
