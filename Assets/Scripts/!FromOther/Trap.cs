// Decompiled with JetBrains decompiler
// Type: Trap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class Trap : MonoBehaviour
{
  [SerializeField]
  private TrapTrigger trapTrigger;

  protected virtual void Start() => this.trapTrigger.AddOnActivateTrapListener(new TrapTrigger.ActivateTrap(this.HandleActivateTrap));

  private void OnDestroy() => this.trapTrigger.DeleteOnActivateTrapListener(new TrapTrigger.ActivateTrap(this.HandleActivateTrap));

  protected abstract void HandleActivateTrap();
}
