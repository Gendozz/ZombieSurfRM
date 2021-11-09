// Decompiled with JetBrains decompiler
// Type: ObstacleGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
  [SerializeField]
  private List<ObstacleNotMy> obstaclePrefabList;
  [SerializeField]
  private float obstacleDensity = 0.5f;
  [SerializeField]
  private List<GameObject> specialObstacleBlockListPrefab;
  [SerializeField]
  private float specialObstacleBlockChange = 0.2f;
  [SerializeField]
  private Billboard billBoardObstaclePrefab;
  [SerializeField]
  private float billBoardObstacleChance = 0.5f;
  [SerializeField]
  private Texture[] billBoardPosters;
  private System.Random billBoardRandom;
  private System.Random billBoardPostersRandom;
  private System.Random specialObstacleBlockRandom;
  private ObstacleList generalObstacleList;
  private ObstacleList lowFloorObstacleList;
  private ObstacleList highFloorObstacleList;
  private ObstacleList springboardObstacleList;

  private void Awake()
  {
    this.specialObstacleBlockRandom = RandomProvider.GetThreadRandom();
    this.billBoardRandom = RandomProvider.GetThreadRandom();
    this.billBoardPostersRandom = RandomProvider.GetThreadRandom();
    this.generalObstacleList = new ObstacleList();
    this.lowFloorObstacleList = new ObstacleList();
    this.highFloorObstacleList = new ObstacleList();
    this.springboardObstacleList = new ObstacleList();
    for (int index = 0; index < this.obstaclePrefabList.Count; ++index)
    {
      if (this.obstaclePrefabList[index].Purpose != ObstacleNotMy.ObstaclePurpose.SpringboardOnly)
      {
        this.generalObstacleList.Add(this.obstaclePrefabList[index]);
        if (this.obstaclePrefabList[index].IsHighFloorObstacle)
          this.highFloorObstacleList.Add(this.obstaclePrefabList[index]);
        else
          this.lowFloorObstacleList.Add(this.obstaclePrefabList[index]);
      }
      if (this.obstaclePrefabList[index].Purpose == ObstacleNotMy.ObstaclePurpose.SpringboardOnly || this.obstaclePrefabList[index].Purpose == ObstacleNotMy.ObstaclePurpose.GeneralAndSpringboard)
        this.springboardObstacleList.Add(this.obstaclePrefabList[index]);
    }
  }

  public void GenerateObstacles(
    Transform roadBlock,
    Vector3 firstCellPosition,
    float laneWidth,
    ObstacleMatrix obstacleMatrix)
  {
    Vector3 vector3 = firstCellPosition;
    for (int i = 0; i < obstacleMatrix.LaneCount; ++i)
    {
      vector3.x = firstCellPosition.x + (float) i * laneWidth;
      int j = 0;
      while (j < obstacleMatrix.LaneCellCount)
      {
        ObstacleMatrixCell obstacleMatrixCell = obstacleMatrix[i, j];
        if ((UnityEngine.Object) obstacleMatrixCell.obstaclePrefab != (UnityEngine.Object) null)
        {
          ObstacleNotMy obstaclePrefab = obstacleMatrixCell.obstaclePrefab;
          vector3.z = (float) ((double) firstCellPosition.z + (double) j + (double) obstaclePrefab.CellCount / 2.0 - 1.0);
          UnityEngine.Object.Instantiate<ObstacleNotMy>(obstacleMatrixCell.obstaclePrefab, roadBlock.position + vector3, Quaternion.identity, roadBlock);
          j += obstaclePrefab.CellCount;
        }
        else
          ++j;
      }
    }
    if (this.billBoardRandom.NextDouble() > (double) this.billBoardObstacleChance)
      return;
    UnityEngine.Object.Instantiate<Billboard>(this.billBoardObstaclePrefab, roadBlock.position, Quaternion.identity, roadBlock).ChangeTexture(this.billBoardPosters[this.billBoardPostersRandom.Next(this.billBoardPosters.Length)]);
  }

  public bool CheckSpecialObstacleBlockChance() => this.specialObstacleBlockRandom.NextDouble() <= (double) this.specialObstacleBlockChange;

  public GameObject GenerateSpecialObstacleBlock(Transform parent, Vector3 position) => UnityEngine.Object.Instantiate<GameObject>(this.specialObstacleBlockListPrefab.Count <= 1 ? this.specialObstacleBlockListPrefab[0] : this.specialObstacleBlockListPrefab[this.specialObstacleBlockRandom.Next(this.specialObstacleBlockListPrefab.Count)], position, Quaternion.identity, parent.transform);

  public ObstacleList AllObstacleList => this.generalObstacleList;

  public ObstacleList LowFloorObstacleList => this.lowFloorObstacleList;

  public ObstacleList HighFloorObstacleList => this.highFloorObstacleList;

  public ObstacleList SpringboardObstacleList => this.springboardObstacleList;

  public float ObstacleDensity => this.obstacleDensity;
}
