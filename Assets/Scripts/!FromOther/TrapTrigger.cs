// Decompiled with JetBrains decompiler
// Type: TrapTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
  private event TrapTrigger.ActivateTrap OnActivateTrap;

  public void CallOnActivateTrap()
  {
    TrapTrigger.ActivateTrap onActivateTrap = this.OnActivateTrap;
    if (onActivateTrap == null)
      return;
    onActivateTrap();
  }

  public void AddOnActivateTrapListener(TrapTrigger.ActivateTrap listener) => this.OnActivateTrap += listener;

  public void DeleteOnActivateTrapListener(TrapTrigger.ActivateTrap listener) => this.OnActivateTrap -= listener;

  private void OnTriggerEnter(Collider other)
  {
    if (!((Object) other.gameObject == (Object) Player.Instance.gameObject))
      return;
    this.CallOnActivateTrap();
  }

  public delegate void ActivateTrap();
}
