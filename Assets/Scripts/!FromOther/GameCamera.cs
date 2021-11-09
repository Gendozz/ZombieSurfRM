// Decompiled with JetBrains decompiler
// Type: GameCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GameCamera : PursuingObject
{
  public Transform gameOverFocusObject;

  protected override void Start()
  {
    base.Start();
    Events.AddOnGameOverListener(new Events.GameOver(this.HandleGameOver));
  }

  private void OnDestroy() => Events.DeleteOnGameOverListener(new Events.GameOver(this.HandleGameOver));

  private void HandleGameOver() => this.ChangeObject(this.gameOverFocusObject);
}
