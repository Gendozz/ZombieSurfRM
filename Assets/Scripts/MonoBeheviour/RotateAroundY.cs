using UnityEngine;

public class RotateAroundY : MonoBehaviour
{
    void Update()
    {
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90);
    }
}
