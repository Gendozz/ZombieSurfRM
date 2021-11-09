// Decompiled with JetBrains decompiler
// Type: ObstacleList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ObstacleList
{
  private List<ObstacleNotMy> obstacles;
  private int minObstacleCellCount;

  public ObstacleList()
  {
    this.obstacles = new List<ObstacleNotMy>();
    this.minObstacleCellCount = 0;
  }

  public void Add(ObstacleNotMy obstacle)
  {
    this.obstacles.Add(obstacle);
    if (this.minObstacleCellCount == 0 || obstacle.CellCount < this.minObstacleCellCount)
      this.minObstacleCellCount = obstacle.CellCount;
    if (this.minObstacleCellCount != 0)
      return;
    Debug.Log((object) "minObstacleCellCount == 0");
    Debug.Break();
  }

  public int Count => this.obstacles.Count;

  public int MinObstacleCellCount => this.minObstacleCellCount;

  public ObstacleNotMy this[int index] => this.obstacles[index];
}
