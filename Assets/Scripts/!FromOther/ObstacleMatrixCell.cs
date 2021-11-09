// Decompiled with JetBrains decompiler
// Type: ObstacleMatrixCell
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public struct ObstacleMatrixCell
{
  public ObstacleNotMy obstaclePrefab;
  public int obstacleCellIndex;

  public void SetFields(ObstacleNotMy obstaclePrefab, int obstacleCellIndex)
  {
    this.obstaclePrefab = obstaclePrefab;
    this.obstacleCellIndex = obstacleCellIndex;
  }

  public void SetAsEmpty() => this.SetFields((ObstacleNotMy) null, 0);

  public bool IsEmpty() => (Object) this.obstaclePrefab == (Object) null;
}
