// Decompiled with JetBrains decompiler
// Type: RestartPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using System;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartPanel : MonoBehaviour
{
  [SerializeField]
  private TextMeshProUGUI scoreText;
  [SerializeField]
  private GameObject highscorePanel;
  [SerializeField]
  private TextMeshProUGUI highscoreText;
  [SerializeField]
  private GameObject newHighscoreMessageText;
  [SerializeField]
  private string highscoreFileName = "highscore.txt";
  private int highscore = -1;

  private void Update()
  {
    if (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Return))
      return;
    this.Restart();
  }

  public void Show(int score)
  {
    this.gameObject.SetActive(true);
    this.scoreText.text = Convert.ToString(score);
    if (this.highscore == -1)
    {
      this.highscore = this.ReadHighscore(this.highscoreFileName);
      this.highscoreText.text = Convert.ToString(this.highscore);
    }
    if (score > this.highscore)
    {
      this.highscore = score;
      this.highscoreText.text = Convert.ToString(score);
      this.highscorePanel.SetActive(false);
      this.newHighscoreMessageText.SetActive(true);
      this.WriteHighscore(this.highscoreFileName, this.highscore);
    }
    else
    {
      this.highscorePanel.SetActive(true);
      this.newHighscoreMessageText.SetActive(false);
    }
  }

  private int ReadHighscore(string fileName)
  {
    int result = 0;
    try
    {
      if (!new FileInfo(fileName).Exists)
        return result;
      using (StreamReader streamReader = new StreamReader(fileName, Encoding.Unicode))
      {
        string s;
        if ((s = streamReader.ReadLine()) != null)
        {
          if (!int.TryParse(s, out result))
            Debug.Log((object) "Incorrect highscore in file!");
        }
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
    return result;
  }

  private void WriteHighscore(string fileName, int score)
  {
    try
    {
      using (StreamWriter streamWriter = new StreamWriter(fileName, false, Encoding.Unicode))
        streamWriter.WriteLine(Convert.ToString(score));
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
