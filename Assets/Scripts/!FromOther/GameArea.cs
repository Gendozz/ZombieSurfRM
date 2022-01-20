// Decompiled with JetBrains decompiler
// Type: GameArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GameArea : MonoBehaviour
{
  [SerializeField]
  private Transform firstLane;
  [SerializeField]
  private float laneWidth = 2f;
  [SerializeField]
  private int laneCount = 3;
  [SerializeField]
  private RoadGenerator road;
  [SerializeField]
  private float zCoordShiftForResetPosition = 100f;

  public static GameArea Instance { get; private set; }

  private void Awake()
  {
    if ((Object) GameArea.Instance == (Object) null)
      GameArea.Instance = this;
    else
      Object.Destroy((Object) this.gameObject);
  }

  private void OnDestroy()
  {
    if (!((Object) GameArea.Instance == (Object) this))
      return;
    GameArea.Instance = (GameArea) null;
    Events.DeleteOnPlayerMoveListener(new Events.PlayerMove(this.HandlePlayerMove));
  }

  private void Start() => Events.AddOnPlayerMoveListener(new Events.PlayerMove(this.HandlePlayerMove));

  public Vector3 GetLanePosition(int laneIndex) => new Vector3(this.firstLane.position.x + this.laneWidth * (float) laneIndex, 0.0f, this.firstLane.position.z);

  private void HandlePlayerMove(Vector3 playerPosition)
  {
    if ((double) playerPosition.z <= (double) this.zCoordShiftForResetPosition)
      return;
    this.road.enabled = false;
    Player.Instance.enabled = false;
    Player.Instance.ChangePlayerPosition(new Vector3(0.0f, 0.0f, -playerPosition.z));
    this.road.ChangeBlocksPosition(new Vector3(0.0f, 0.0f, -playerPosition.z));
    Player.Instance.enabled = true;
    this.road.enabled = true;
  }

  public float LaneWidth => this.laneWidth;

  public int LaneCount => this.laneCount;
}
