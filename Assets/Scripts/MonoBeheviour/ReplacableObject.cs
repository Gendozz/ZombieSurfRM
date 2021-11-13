using UnityEngine;
using UnityEngine.Events;

public class ReplacableObject : MonoBehaviour, IPooledObject
{
    public Transform endPosition;

    public Vector3Reference firstCreationPosition;

    public UnityEvent objectIsOutOfSee;

    public FloatReference minZ;

    public bool isReplaced = false;

    public void OnObjectSpawn()
    {
        isReplaced = true;
    }

    private void Update()
    {
        if(isReplaced && transform.position.z < minZ.value)
        {
            objectIsOutOfSee?.Invoke();
            isReplaced = false;
        }
    }
}
