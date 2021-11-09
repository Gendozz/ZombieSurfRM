// Decompiled with JetBrains decompiler
// Type: Coin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

public class Coin : Bounty
{
  protected const float PICKUP_TIME = 0.3f;
  protected const float PICKUP_SIZE_MULTIPLIER = 0.3f;
  [SerializeField]
  private int scoreCost = 10;

  protected override void OnTriggerEnter(Collider other)
  {
    base.OnTriggerEnter(other);
    if (!((Object) other.gameObject == (Object) Player.Instance.gameObject))
      return;
    this.objectCollider.enabled = false;
    if ((Object) this.pickUpSound != (Object) null)
    {
      this.pickUpSound.pitch = Random.Range(1f, 1.5f);
      this.pickUpSound.Play();
    }
    if ((Object) this.pickUpEffect != (Object) null)
    {
      this.pickUpEffect.transform.SetParent(this.transform.parent);
      this.pickUpEffect.Play();
    }
    this.StartCoroutine(this.PickUp(0.3f));
    float t = 0.3f;
    if ((Object) this.pickUpSound != (Object) null && (double) this.pickUpSound.clip.length > (double) t)
      t = this.pickUpSound.clip.length;
    Object.Destroy((Object) this.gameObject, t);
  }

  private IEnumerator PickUp(float time)
  {
    Coin coin = this;
    Vector3 worldPoint = Camera.main.ViewportToWorldPoint(new Vector3(-10f, 10f, Camera.main.nearClipPlane));
    worldPoint.z = coin.transform.position.z;
    Vector3 shiftVector = worldPoint - coin.transform.position;
    coin.transform.SetParent(Camera.main.transform);
    float distance = shiftVector.magnitude;
    float distPerFrame = Time.fixedDeltaTime * (distance / time);
    shiftVector = Vector3.Normalize(shiftVector) * distPerFrame;
    Vector3 resizePerFrame = coin.transform.localScale * Time.fixedDeltaTime * ((coin.transform.localScale.x - coin.transform.localScale.x * 0.3f) / time);
    while ((double) distance > 0.0)
    {
      coin.transform.Translate(shiftVector);
      coin.transform.localScale -= resizePerFrame;
      distance -= distPerFrame;
      yield return (object) new WaitForFixedUpdate();
    }
    Events.CallOnCollectBounty(coin.scoreCost);
  }
}
