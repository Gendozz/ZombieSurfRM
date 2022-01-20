// Decompiled with JetBrains decompiler
// Type: RoadGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
  private const float TILE_SIZE = 6f;
  private const float ROAD_TILE_COUNT = 3f;
  private const float SIDE_BLOCK_TILE_COUNT = 3f;
  private const float SIDE_BLOCK_SIZE = 18f;
  private const float SIDE_BLOCK_SHIFT = 12f;
  private const string ROAD_BLOCK_NAME = "RoadBlock";
  [SerializeField]
  private int maxRoadBlockCount = 10;
  [SerializeField]
  private GameObject startRoadBlock;
  [SerializeField]
  private GameObject highwayBlockPrefab;
  [SerializeField]
  private GameObject crossRoadBlockPrefab;
  [SerializeField]
  private GameObject bridgeBlockPrefab;
  [SerializeField]
  private GameObject backSideBlockPrefab;
  [SerializeField]
  private List<GameObject> villageBlockListPrefab;
  [SerializeField]
  private List<GameObject> townBlockListPrefab;
  [SerializeField]
  private int villageAreaMinBlockCount = 4;
  [SerializeField]
  private int villageAreaMaxBlockCount = 6;
  [SerializeField]
  private int townAreaMinBlockCount = 5;
  [SerializeField]
  private int townAreaMaxBlockCount = 7;
  [SerializeField]
  private float crossRoadChance = 0.2f;
  [SerializeField]
  private float bridgeBlockChance = 0.2f;
  private List<GameObject> roadBlockList;
  private RoadGenerator.RoadType currentRoadType;
  private int currentRoadTypeBlocksLeft;
  private float lastRoadBlockLength;
  private ObstacleGeneratorNotMy og;
  private BountyGenerator bg;
  private System.Random blockTypeRandom;
  private System.Random crossRoadRandom;
  private System.Random bridgeBlockRandom;
  private Vector3 firstTileLocalCenter;
  private Vector3 crossRoadTileLocalCenter;
  private int currentGeneratedBlockCount;

  private void Start()
  {
    this.og = this.GetComponent<ObstacleGeneratorNotMy>();
    this.bg = this.GetComponent<BountyGenerator>();
    this.roadBlockList = new List<GameObject>();
    this.blockTypeRandom = RandomProvider.GetThreadRandom();
    this.crossRoadRandom = RandomProvider.GetThreadRandom();
    this.bridgeBlockRandom = RandomProvider.GetThreadRandom();
    this.firstTileLocalCenter = new Vector3(0.0f, 0.0f, -6f);
    this.crossRoadTileLocalCenter = -this.firstTileLocalCenter;
    this.crossRoadTileLocalCenter.z += 6f;
    this.RefreshRoadType();
    this.GenerateRoadBlock(this.startRoadBlock.transform.position, false);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.startRoadBlock);
    this.GenerateRoad();
    Events.AddOnPlayerMoveListener(new Events.PlayerMove(this.HandlePlayerMove));
  }

  private void OnDestroy() => Events.DeleteOnPlayerMoveListener(new Events.PlayerMove(this.HandlePlayerMove));

  private void GenerateRoad()
  {
    while (this.roadBlockList.Count < this.maxRoadBlockCount)
    {
      if (this.roadBlockList.Count > 3 && this.og.CheckSpecialObstacleBlockChance())
      {
        this.GenerateRoadBlockAfterLast(false);
        this.GenerateRoadBlockAfterLast(false, true);
      }
      else
        this.GenerateRoadBlockAfterLast();
    }
  }

  private void GenerateRoadBlockAfterLast(bool generateObstacles = true, bool generateSpecialObstacleBlock = false)
  {
    if (this.roadBlockList.Count == 0)
      return;
    Vector3 position = this.roadBlockList[this.roadBlockList.Count - 1].transform.position;
    position.z += this.lastRoadBlockLength;
    this.GenerateRoadBlock(position, generateObstacles, generateSpecialObstacleBlock);
  }

  private void GenerateRoadBlock(
    Vector3 position,
    bool generateObstacles = true,
    bool generateSpecialObstacleBlock = false)
  {
    ++this.currentGeneratedBlockCount;
    GameObject gameObject = new GameObject("RoadBlock");
    gameObject.transform.SetParent(this.gameObject.transform);
    gameObject.transform.position = position;
    Vector3 position1 = position + this.firstTileLocalCenter;
    for (int index = 0; (double) index < 3.0; ++index)
    {
      UnityEngine.Object.Instantiate<GameObject>(this.highwayBlockPrefab, position1, Quaternion.identity, gameObject.transform);
      position1.z += 6f;
    }
    this.lastRoadBlockLength = 3f;
    if (this.crossRoadRandom.NextDouble() <= (double) this.crossRoadChance)
    {
      position1 = position + this.crossRoadTileLocalCenter;
      UnityEngine.Object.Instantiate<GameObject>(this.highwayBlockPrefab, position1, Quaternion.identity, gameObject.transform);
      UnityEngine.Object.Instantiate<GameObject>(this.crossRoadBlockPrefab, new Vector3(position1.x + 6f, 0.0f, position1.z), Quaternion.identity, gameObject.transform);
      UnityEngine.Object.Instantiate<GameObject>(this.crossRoadBlockPrefab, new Vector3(position1.x - 6f, 0.0f, position1.z), Quaternion.Euler(new Vector3(0.0f, 180f, 0.0f)), gameObject.transform);
      ++this.lastRoadBlockLength;
    }
    this.lastRoadBlockLength *= 6f;
    if (generateSpecialObstacleBlock)
      this.og.GenerateSpecialObstacleBlock(gameObject.transform, position);
    else if (this.currentRoadType == RoadGenerator.RoadType.Town && this.bridgeBlockRandom.NextDouble() <= (double) this.bridgeBlockChance)
    {
      UnityEngine.Object.Instantiate<GameObject>(this.bridgeBlockPrefab, position, Quaternion.identity, gameObject.transform);
    }
    else
    {
      switch (this.currentRoadType)
      {
        case RoadGenerator.RoadType.Town:
          this.GenerateSideBlocks(gameObject.transform, this.townBlockListPrefab);
          break;
        case RoadGenerator.RoadType.Village:
          this.GenerateSideBlocks(gameObject.transform, this.villageBlockListPrefab);
          break;
      }
    }
    int laneCount = GameArea.Instance.LaneCount;
    float laneWidth = GameArea.Instance.LaneWidth;
    if (generateObstacles | generateSpecialObstacleBlock)
    {
      int laneCellCount = Mathf.CeilToInt(this.lastRoadBlockLength);
      Vector3 firstCellPosition = new Vector3(-laneWidth, 0.0f, this.firstTileLocalCenter.z - 2f);
      ObstacleMatrix obstacleMatrix = new ObstacleMatrix(this.og, laneCount, laneCellCount);
      if (!generateSpecialObstacleBlock)
        this.og.GenerateObstacles(gameObject.transform, firstCellPosition, laneWidth, obstacleMatrix);
      this.bg.GenerateBounty(this.currentGeneratedBlockCount, gameObject.transform, firstCellPosition, laneWidth, obstacleMatrix);
    }
    this.roadBlockList.Add(gameObject);
    --this.currentRoadTypeBlocksLeft;
    if (this.currentRoadTypeBlocksLeft > 0)
      return;
    this.RefreshRoadType();
  }

  private void GenerateSideBlocks(Transform parent, List<GameObject> sideBlockPrefabList)
  {
    GameObject sideBlockPrefab1 = sideBlockPrefabList[this.blockTypeRandom.Next(sideBlockPrefabList.Count)];
    this.GenerateSideBlock(parent, sideBlockPrefab1, 1);
    GameObject sideBlockPrefab2 = sideBlockPrefabList[this.blockTypeRandom.Next(sideBlockPrefabList.Count)];
    this.GenerateSideBlock(parent, sideBlockPrefab2, -1);
  }

  private void GenerateSideBlock(Transform parent, GameObject sideBlockPrefab, int direction)
  {
    Quaternion rotation = direction >= 0 ? Quaternion.identity : Quaternion.Euler(new Vector3(0.0f, 180f, 0.0f));
    Vector3 vector3 = new Vector3((float) direction * 12f, 0.0f, 0.0f);
    UnityEngine.Object.Instantiate<GameObject>(sideBlockPrefab, parent.position + vector3, rotation, parent);
    vector3.x += (float) direction * 18f;
    UnityEngine.Object.Instantiate<GameObject>(this.backSideBlockPrefab, parent.position + vector3, rotation, parent);
  }

  private void RefreshRoadType()
  {
    this.currentRoadType = (RoadGenerator.RoadType) this.blockTypeRandom.Next(Enum.GetValues(typeof (RoadGenerator.RoadType)).Length);
    switch (this.currentRoadType)
    {
      case RoadGenerator.RoadType.Town:
        this.currentRoadTypeBlocksLeft = this.blockTypeRandom.Next(this.townAreaMinBlockCount, this.townAreaMaxBlockCount + 1);
        break;
      case RoadGenerator.RoadType.Village:
        this.currentRoadTypeBlocksLeft = this.blockTypeRandom.Next(this.villageAreaMinBlockCount, this.villageAreaMaxBlockCount + 1);
        break;
      default:
        this.currentRoadTypeBlocksLeft = 1;
        break;
    }
  }

  public void ChangeBlocksPosition(Vector3 shiftVector)
  {
    for (int index = 0; index < this.roadBlockList.Count; ++index)
    {
      this.roadBlockList[index].SetActive(false);
      this.roadBlockList[index].transform.Translate(shiftVector);
      this.roadBlockList[index].SetActive(true);
    }
  }

  private void HandlePlayerMove(Vector3 playerPosition)
  {
    if (this.roadBlockList.Count > 2 && (double) playerPosition.z > (double) this.roadBlockList[2].transform.position.z)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.roadBlockList[0]);
      this.roadBlockList.RemoveAt(0);
    }
    if (this.roadBlockList.Count >= this.maxRoadBlockCount)
      return;
    this.GenerateRoad();
  }

  private enum RoadType
  {
    Town,
    Village,
  }
}
