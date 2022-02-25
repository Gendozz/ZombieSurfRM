using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceTrap : Trap
{
    public override void ActivateTrap()
    {
        usedRigidbody.useGravity = true;
    }

    protected override void OnDisable()
    {
        usedRigidbody.useGravity = false;
        base.OnDisable();
    }
}
