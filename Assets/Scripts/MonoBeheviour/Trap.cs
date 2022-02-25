using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    [SerializeField]
    protected Transform usedTransform;

    [SerializeField]
    protected Rigidbody usedRigidbody;

    protected Quaternion defaultRotation;

    protected Vector3 defaultPosition;

    public abstract void ActivateTrap();

    protected void Awake()
    {
        defaultRotation = usedTransform.rotation;
        defaultPosition = usedTransform.position;
    }

    protected virtual void OnDisable()
    {
        ReturnDefaults();
    }

    protected virtual void ReturnDefaults()
    {
        usedRigidbody.velocity = Vector3.zero;
        usedRigidbody.angularVelocity = Vector3.zero;
        usedTransform.rotation = defaultRotation;
        usedTransform.localPosition = defaultPosition;
    }
}
