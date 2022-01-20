// Decompiled with JetBrains decompiler
// Type: Manhole
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Manhole : Trap
{
  [SerializeField]
  private FixedJoint fixedJoint;
  [SerializeField]
  private ParticleSystem gasEffect;

  protected override void Start() => base.Start();

  protected override void HandleActivateTrap()
  {
    Object.Destroy((Object) this.fixedJoint);
    this.gasEffect.Play();
    ParticleSystem.MainModule main = this.gasEffect.main;
    main.playOnAwake = true;
    main.prewarm = true;
  }
}
