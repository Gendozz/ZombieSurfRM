using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchTrap : Trap
{
    [SerializeField]
    private float force = 100f;

    public override void ActivateTrap()
    {
        usedRigidbody.AddForce(Vector3.forward * force, ForceMode.Impulse);
    }
}
