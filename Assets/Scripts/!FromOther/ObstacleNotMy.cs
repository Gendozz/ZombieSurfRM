// Decompiled with JetBrains decompiler
// Type: Obstacle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ObstacleNotMy : MonoBehaviour
{
  [SerializeField]
  private Transform dimentionsBox;
  [SerializeField]
  private bool isHighFloorObstacle;
  [SerializeField]
  private ObstacleNotMy.ObstaclePurpose purpose;
  [SerializeField]
  private Transform bountySpawnPosition;

  public int CellCount => Mathf.CeilToInt(this.dimentionsBox.lossyScale.z);

  public bool IsHighFloorObstacle => this.isHighFloorObstacle;

  public ObstacleNotMy.ObstaclePurpose Purpose => this.purpose;

  public Transform BountySpawnPosition => this.bountySpawnPosition;

  public enum ObstaclePurpose
  {
    General,
    SpringboardOnly,
    GeneralAndSpringboard,
  }
}
