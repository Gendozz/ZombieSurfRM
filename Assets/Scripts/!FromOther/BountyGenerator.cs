// Decompiled with JetBrains decompiler
// Type: BountyGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class BountyGenerator : MonoBehaviour
{
  private const float SPAWN_HEIGHT = 5f;
  private const float ROTATION_STEP = 20f;
  [SerializeField]
  private Coin coinPrefab;
  [SerializeField]
  private float lowestBountyChance = 0.1f;
  [SerializeField]
  private float highestBountyChance = 0.9f;
  [SerializeField]
  private Bounty jumpBonusPrefab;
  [SerializeField]
  private int roadBlockCountBeforejumpBonusCanAppear = 3;
  [SerializeField]
  private float jumpBonusChance = 0.3f;
  private System.Random coinRandom;
  private System.Random bonusRandom;

  private void Awake()
  {
    this.coinRandom = RandomProvider.GetThreadRandom();
    this.bonusRandom = RandomProvider.GetThreadRandom();
  }

  public void GenerateCoins(
    Transform roadBlock,
    Vector3 firstCellPosition,
    int[] bountyPathLaneIndexArray,
    float laneWidth)
  {
    float num = this.lowestBountyChance;
    for (int laneCellIndex = 0; laneCellIndex < bountyPathLaneIndexArray.Length; ++laneCellIndex)
    {
      if ((double) num > this.coinRandom.NextDouble())
      {
        num = this.highestBountyChance;
        this.CreateBounty((Bounty) this.coinPrefab, roadBlock, firstCellPosition, bountyPathLaneIndexArray[laneCellIndex], laneCellIndex, laneWidth).SetRotation((float) laneCellIndex * 20f);
      }
      else
        num = this.lowestBountyChance;
    }
  }

  public void GenerateBonus(
    Transform roadBlock,
    Vector3 firstCellPosition,
    int[] bountyPathLaneIndexArray,
    float laneWidth)
  {
    int laneCellIndex = this.coinRandom.Next(bountyPathLaneIndexArray.Length);
    this.CreateBounty(this.jumpBonusPrefab, roadBlock, firstCellPosition, bountyPathLaneIndexArray[laneCellIndex], laneCellIndex, laneWidth);
  }

  public void GenerateBounty(
    int currentGeneratedBlockCount,
    Transform roadBlock,
    Vector3 firstCellPosition,
    float laneWidth,
    ObstacleMatrix obstacleMatrix)
  {
    int[] pathLaneIndexArray = new BountyPath(obstacleMatrix, this.coinRandom).BountyPathLaneIndexArray;
    if (currentGeneratedBlockCount % this.roadBlockCountBeforejumpBonusCanAppear == 0 && (double) this.jumpBonusChance > this.bonusRandom.NextDouble())
      this.GenerateBonus(roadBlock, firstCellPosition, pathLaneIndexArray, laneWidth);
    else
      this.GenerateCoins(roadBlock, firstCellPosition, pathLaneIndexArray, laneWidth);
  }

  public Bounty CreateBounty(
    Bounty prefab,
    Transform roadBlock,
    Vector3 firstCellPosition,
    int laneIndex,
    int laneCellIndex,
    float laneWidth)
  {
    Vector3 vector3;
    vector3.x = firstCellPosition.x + (float) laneIndex * laneWidth;
    vector3.y = 5f;
    vector3.z = (float) ((double) firstCellPosition.z + (double) laneCellIndex - 0.5);
    return UnityEngine.Object.Instantiate<Bounty>(prefab, roadBlock.position + vector3, Quaternion.identity, roadBlock);
  }
}
