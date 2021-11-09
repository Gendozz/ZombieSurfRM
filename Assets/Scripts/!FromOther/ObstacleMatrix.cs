// Decompiled with JetBrains decompiler
// Type: ObstacleMatrix
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ObstacleMatrix
{
  private ObstacleMatrixCell[,] matrix;
  private static System.Random random;

  public static System.Random Random
  {
    get
    {
      if (ObstacleMatrix.random == null)
        ObstacleMatrix.random = RandomProvider.GetThreadRandom();
      return ObstacleMatrix.random;
    }
  }

  public ObstacleMatrix(ObstacleGenerator og, int laneCount, int laneCellCount)
  {
    if (laneCount <= 0 || laneCellCount <= 0)
      return;
    this.matrix = this.CreateEmptyObstacleMatrix(laneCount, laneCellCount);
    int[] indexArray = CommonFunctions.CreateIndexArray(laneCount);
    CommonFunctions.ShuffleArray(indexArray, ObstacleMatrix.Random);
    int num1 = laneCount - 1;
    int num2 = 0;
    for (int index = 0; index < laneCount; ++index)
    {
      int laneIndex = indexArray[index];
      int startLaneCellIndex = 0;
      ObstacleNotMy obstacle1 = (ObstacleNotMy) null;
      while (startLaneCellIndex < laneCellCount)
      {
        if (ObstacleMatrix.Random.NextDouble() > (double) og.ObstacleDensity)
        {
          startLaneCellIndex += 2;
          obstacle1 = (ObstacleNotMy) null;
        }
        else
        {
          int maxObstacleCellCount1 = laneCellCount - startLaneCellIndex;
          ObstacleNotMy obstacle2 = num2 >= num1 ? this.TryGetRandomObstacle(og.LowFloorObstacleList, maxObstacleCellCount1) : this.TryGetRandomObstacle(og.AllObstacleList, maxObstacleCellCount1);
          if (!((UnityEngine.Object) obstacle2 == (UnityEngine.Object) null))
          {
            if (obstacle2.IsHighFloorObstacle)
              ++num2;
            if (obstacle2.IsHighFloorObstacle && ((UnityEngine.Object) obstacle1 != (UnityEngine.Object) null && (obstacle1.Purpose == ObstacleNotMy.ObstaclePurpose.GeneralAndSpringboard || obstacle1.Purpose == ObstacleNotMy.ObstaclePurpose.SpringboardOnly) || ObstacleMatrix.Random.NextDouble() > 0.5))
            {
              int maxObstacleCellCount2 = maxObstacleCellCount1 - obstacle2.CellCount;
              ObstacleNotMy randomObstacle = this.TryGetRandomObstacle(og.SpringboardObstacleList, maxObstacleCellCount2);
              if ((UnityEngine.Object) randomObstacle != (UnityEngine.Object) null)
              {
                this.AddObstacleToMatrix(this.matrix, laneIndex, startLaneCellIndex, randomObstacle);
                startLaneCellIndex += randomObstacle.CellCount;
              }
            }
            this.AddObstacleToMatrix(this.matrix, laneIndex, startLaneCellIndex, obstacle2);
            startLaneCellIndex += obstacle2.CellCount;
            obstacle1 = obstacle2;
          }
          else
            break;
        }
      }
    }
  }

  private ObstacleNotMy TryGetRandomObstacle(ObstacleList obstacleList, int maxObstacleCellCount)
  {
    if (obstacleList.MinObstacleCellCount == 0)
    {
      Debug.Log((object) obstacleList);
      Debug.Break();
    }
    if (obstacleList.MinObstacleCellCount > maxObstacleCellCount)
      return (ObstacleNotMy) null;
    ObstacleNotMy obstacle;
    do
    {
      obstacle = obstacleList[ObstacleMatrix.Random.Next(obstacleList.Count)];
    }
    while (obstacle.CellCount > maxObstacleCellCount);
    return obstacle;
  }

  private ObstacleMatrixCell[,] CreateEmptyObstacleMatrix(
    int laneCount,
    int laneCellCount)
  {
    ObstacleMatrixCell[,] obstacleMatrixCellArray = new ObstacleMatrixCell[laneCount, laneCellCount];
    for (int index1 = 0; index1 < laneCount; ++index1)
    {
      for (int index2 = 0; index2 < laneCellCount; ++index2)
        obstacleMatrixCellArray[index1, index2].SetAsEmpty();
    }
    return obstacleMatrixCellArray;
  }

  private void AddObstacleToMatrix(
    ObstacleMatrixCell[,] obstacleMatrix,
    int laneIndex,
    int startLaneCellIndex,
    ObstacleNotMy obstacle)
  {
    for (int obstacleCellIndex = 0; obstacleCellIndex < obstacle.CellCount; ++obstacleCellIndex)
      obstacleMatrix[laneIndex, startLaneCellIndex + obstacleCellIndex].SetFields(obstacle, obstacleCellIndex);
  }

  public int LaneCount => this.matrix.GetLength(0);

  public int LaneCellCount => this.matrix.GetLength(1);

  public ObstacleMatrixCell this[int i, int j] => this.matrix[i, j];
}
