// Decompiled with JetBrains decompiler
// Type: BountyPath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BountyPath
{
  private ObstacleMatrix obstacleMatrix;
  private int[] bountyPathLaneIndexArray;
  private System.Random random;
  private int[] laneIndexArray;
  private List<int> possibleLaneList;

  public BountyPath(ObstacleMatrix obstacleMatrix, System.Random random)
  {
    if (obstacleMatrix == null || obstacleMatrix.LaneCount == 0 || obstacleMatrix.LaneCellCount == 0)
      return;
    this.obstacleMatrix = obstacleMatrix;
    this.random = random;
    this.laneIndexArray = CommonFunctions.CreateIndexArray(obstacleMatrix.LaneCount);
    this.possibleLaneList = new List<int>();
    BountyPath.BountyMatrixCell[,] bountyMatrix = this.CreateBountyMatrix(obstacleMatrix);
    bountyMatrix.GetLength(0);
    int length = bountyMatrix.GetLength(1);
    this.bountyPathLaneIndexArray = new int[length];
    int laneCellIndex = 0;
    while (laneCellIndex < length)
    {
      int previousLaneIndex = laneCellIndex == 0 ? -1 : this.bountyPathLaneIndexArray[laneCellIndex - 1];
      int reachableLaneIndex = this.TryGetRandomReachableLaneIndex(bountyMatrix, laneCellIndex, previousLaneIndex);
      if (reachableLaneIndex == -1)
      {
        --laneCellIndex;
        if (laneCellIndex < 0)
        {
          Debug.Log((object) "BountyMatrix rowIndex < 0");
          Debug.DebugBreak();
          break;
        }
        bountyMatrix[previousLaneIndex, laneCellIndex].isDeadEnd = true;
      }
      else
      {
        this.bountyPathLaneIndexArray[laneCellIndex] = reachableLaneIndex;
        ++laneCellIndex;
      }
    }
  }

  private BountyPath.BountyMatrixCell[,] CreateBountyMatrix(ObstacleMatrix obstacleMatrix)
  {
    BountyPath.BountyMatrixCell[,] bountyMatrixCellArray = new BountyPath.BountyMatrixCell[obstacleMatrix.LaneCount, obstacleMatrix.LaneCellCount];
    for (int index1 = 0; index1 < bountyMatrixCellArray.GetLength(0); ++index1)
    {
      for (int index2 = 0; index2 < bountyMatrixCellArray.GetLength(1); ++index2)
        bountyMatrixCellArray[index1, index2].isDeadEnd = false;
    }
    return bountyMatrixCellArray;
  }

  private int TryGetRandomReachableLaneIndex(
    BountyPath.BountyMatrixCell[,] bountyMatrix,
    int laneCellIndex,
    int previousLaneIndex)
  {
    int num1 = -1;
    if (laneCellIndex < 0 || laneCellIndex >= this.obstacleMatrix.LaneCellCount)
      return num1;
    bool flag = previousLaneIndex >= 0 && laneCellIndex > 0;
    ObstacleNotMy obstacle = !flag ? (ObstacleNotMy) null : this.obstacleMatrix[previousLaneIndex, laneCellIndex - 1].obstaclePrefab;
    CommonFunctions.ShuffleArray(this.laneIndexArray, this.random);
    this.possibleLaneList.Clear();
    for (int index = 0; index < this.laneIndexArray.Length; ++index)
    {
      int laneIndex = this.laneIndexArray[index];
      if (!bountyMatrix[laneIndex, laneCellIndex].isDeadEnd)
      {
        int num2 = !flag ? 0 : laneIndex - previousLaneIndex;
        if (!flag || Mathf.Abs(num2) <= 1)
        {
          if ((UnityEngine.Object) obstacle != (UnityEngine.Object) null && (obstacle.IsHighFloorObstacle || obstacle.Purpose != ObstacleNotMy.ObstaclePurpose.General))
            this.possibleLaneList.Add(laneIndex);
          else if (this.CanMoveOnObstacleCell(this.obstacleMatrix[laneIndex, laneCellIndex].obstaclePrefab))
          {
            if (!flag || num2 == 0)
            {
              this.possibleLaneList.Add(laneIndex);
            }
            else
            {
              ObstacleNotMy obstaclePrefab1 = this.obstacleMatrix[previousLaneIndex, laneCellIndex].obstaclePrefab;
              ObstacleNotMy obstaclePrefab2 = this.obstacleMatrix[laneIndex, laneCellIndex - 1].obstaclePrefab;
              if (this.CanMoveOnObstacleCell(obstaclePrefab1) && this.CanMoveOnObstacleCell(obstaclePrefab2))
                this.possibleLaneList.Add(laneIndex);
            }
          }
        }
      }
    }
    if (this.possibleLaneList.Count == 0)
      return num1;
    if (this.possibleLaneList.Count == 1)
      return this.possibleLaneList[0];
    for (int index = 0; index < this.possibleLaneList.Count; ++index)
    {
      ObstacleNotMy obstaclePrefab = this.obstacleMatrix[this.possibleLaneList[index], laneCellIndex].obstaclePrefab;
      if ((UnityEngine.Object) obstaclePrefab != (UnityEngine.Object) null && (obstaclePrefab.IsHighFloorObstacle || obstaclePrefab.Purpose == ObstacleNotMy.ObstaclePurpose.SpringboardOnly))
      {
        num1 = this.possibleLaneList[index];
        break;
      }
    }
    if (num1 != -1)
      return num1;
    for (int index = 0; index < this.possibleLaneList.Count; ++index)
    {
      ObstacleNotMy obstaclePrefab = this.obstacleMatrix[this.possibleLaneList[index], laneCellIndex].obstaclePrefab;
      if ((UnityEngine.Object) obstaclePrefab != (UnityEngine.Object) null && obstaclePrefab.Purpose != ObstacleNotMy.ObstaclePurpose.General)
      {
        num1 = this.possibleLaneList[index];
        break;
      }
    }
    return num1 != -1 ? num1 : (this.possibleLaneList.IndexOf(previousLaneIndex) == -1 ? this.possibleLaneList[0] : previousLaneIndex);
  }

  private bool CanMoveOnObstacleCell(ObstacleNotMy obstacle) => (UnityEngine.Object) obstacle == (UnityEngine.Object) null || !obstacle.IsHighFloorObstacle;

  public int[] BountyPathLaneIndexArray => this.bountyPathLaneIndexArray;

  private struct BountyMatrixCell
  {
    public bool isDeadEnd;
  }
}
