// Decompiled with JetBrains decompiler
// Type: DeathTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    if (!((Object) other.gameObject == (Object) Player.Instance.gameObject))
      return;
    Player.Instance.Kill();
  }
}
