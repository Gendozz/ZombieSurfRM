// Decompiled with JetBrains decompiler
// Type: GameInterface
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class GameInterface : MonoBehaviour
{
  [SerializeField]
  public TextMeshProUGUI scoreText;
  [SerializeField]
  public RestartPanel restartPanel;
  private int score;

  public static GameInterface Instance { get; private set; }

  private void Awake()
  {
    if ((Object) GameInterface.Instance == (Object) null)
      GameInterface.Instance = this;
    else
      Object.Destroy((Object) this.gameObject);
  }

  private void OnDestroy()
  {
    if (!((Object) GameInterface.Instance == (Object) this))
      return;
    GameInterface.Instance = (GameInterface) null;
    Events.DeleteOnGameOverListener(new Events.GameOver(this.HandleGameOver));
    Events.DeleteOnCollectBountyListener(new Events.CollectBounty(this.AddScore));
  }

  private void Start()
  {
    Cursor.visible = false;
    this.restartPanel.gameObject.SetActive(false);
    this.ResetScore();
    Events.AddOnGameOverListener(new Events.GameOver(this.HandleGameOver));
    Events.AddOnCollectBountyListener(new Events.CollectBounty(this.AddScore));
  }

  private void Update()
  {
    if (!Input.GetKey(KeyCode.Escape))
      return;
    Application.Quit();
  }

  private void RefreshScoreText() => this.scoreText.text = this.score.ToString();

  private void ResetScore()
  {
    this.score = 0;
    this.RefreshScoreText();
  }

  public void AddScore(int scoreToAdd)
  {
    this.score += scoreToAdd;
    this.RefreshScoreText();
  }

  private void ShowRestartPanel()
  {
    Cursor.visible = true;
    this.restartPanel.Show(this.score);
  }

  private void HandleGameOver() => this.Invoke("ShowRestartPanel", 2f);
}
