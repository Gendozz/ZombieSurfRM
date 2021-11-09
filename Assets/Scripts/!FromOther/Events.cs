// Decompiled with JetBrains decompiler
// Type: Events
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class Events
{
  private static event Events.PlayerMove OnPlayerMove;

  private static event Events.GameOver OnGameOver;

  private static event Events.CollectBounty OnCollectBounty;

  private static event Events.CollectJumpBonus OnCollectJumpBonus;

  public static void CallOnPlayerMove(Vector3 playerPosition)
  {
    Events.PlayerMove onPlayerMove = Events.OnPlayerMove;
    if (onPlayerMove == null)
      return;
    onPlayerMove(playerPosition);
  }

  public static void AddOnPlayerMoveListener(Events.PlayerMove listener) => Events.OnPlayerMove += listener;

  public static void DeleteOnPlayerMoveListener(Events.PlayerMove listener) => Events.OnPlayerMove -= listener;

  public static void CallOnGameOver()
  {
    Events.GameOver onGameOver = Events.OnGameOver;
    if (onGameOver == null)
      return;
    onGameOver();
  }

  public static void AddOnGameOverListener(Events.GameOver listener) => Events.OnGameOver += listener;

  public static void DeleteOnGameOverListener(Events.GameOver listener) => Events.OnGameOver -= listener;

  public static void CallOnCollectBounty(int score)
  {
    Events.CollectBounty onCollectBounty = Events.OnCollectBounty;
    if (onCollectBounty == null)
      return;
    onCollectBounty(score);
  }

  public static void AddOnCollectBountyListener(Events.CollectBounty listener) => Events.OnCollectBounty += listener;

  public static void DeleteOnCollectBountyListener(Events.CollectBounty listener) => Events.OnCollectBounty -= listener;

  public static void CallOnCollectJumpBonus()
  {
    Events.CollectJumpBonus collectJumpBonus = Events.OnCollectJumpBonus;
    if (collectJumpBonus == null)
      return;
    collectJumpBonus();
  }

  public static void AddOnCollectJumpBonusListener(Events.CollectJumpBonus listener) => Events.OnCollectJumpBonus += listener;

  public static void DeleteOnCollectJumpBonusListener(Events.CollectJumpBonus listener) => Events.OnCollectJumpBonus -= listener;

  public delegate void PlayerMove(Vector3 playerPosition);

  public delegate void GameOver();

  public delegate void CollectBounty(int score);

  public delegate void CollectJumpBonus();
}
