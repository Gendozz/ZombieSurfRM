// Decompiled with JetBrains decompiler
// Type: Bonus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

public class Bonus : Bounty
{
  protected override void OnTriggerEnter(Collider other)
  {
    base.OnTriggerEnter(other);
    if (!((Object) other.gameObject == (Object) Player.Instance.gameObject))
      return;
    this.objectCollider.enabled = false;
    this.pickUpSound?.Play();
    this.pickUpEffect?.Play();
    this.initialEffect?.Stop();
    this.StartCoroutine(this.PickUp(0.5f));
    Object.Destroy((Object) this.gameObject, this.initialEffect.main.duration);
  }

  private IEnumerator PickUp(float time)
  {
    Bonus bonus = this;
    Events.CallOnCollectJumpBonus();
    Vector3 resizePerFrame = bonus.transform.localScale * Time.fixedDeltaTime * (bonus.transform.localScale.x / time);
    while ((double) bonus.transform.localScale.x > 0.0)
    {
      bonus.transform.localScale -= resizePerFrame;
      yield return (object) new WaitForFixedUpdate();
    }
  }
}
