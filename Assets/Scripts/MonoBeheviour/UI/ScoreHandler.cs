using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    // Display scores
    [SerializeField]
    private Text scoreText;

    private readonly string coinText = "Coins: ";

    [SerializeField]
    private int currentScores;

    // Display highscores
    [SerializeField]
    private Text displayNewHighscore;

    [SerializeField]
    private Text[] topFiveHighscores;

    // Save scores
    private List<ScoreNote> highscores = new List<ScoreNote>();

    private int highscoresAmount = 0;

    [SerializeField]
    private StringReference playerName;

    private void Awake()
    {
        LoadScores();
    }

    public void UpdateDisplayedScore()
    {
        currentScores++;
        scoreText.text = coinText + currentScores;

        if(highscores.Count > 0 && currentScores > highscores[0].Score)
        {
            displayNewHighscore.gameObject.SetActive(true);
        }
    }

    // Call OnDeath UnityEvent
    public void SaveScore()
    {
        //TODO: Save stats
        ScoreNote currentScoreNote = new ScoreNote(playerName.GetValue(), currentScores);

        highscores.Add(currentScoreNote);

        highscores.Sort((a, b)=> b.CompareTo(a)); // Sort Descending

        ScoreSaver.SaveScores(highscores);

        if(highscores.Count > 0)
        {
            highscoresAmount = highscores.Count;
        }

        if(highscoresAmount > 5)
        {
            highscoresAmount = 5;
        }

        for (int i = 0; i < highscoresAmount; i++)
        {
            topFiveHighscores[i].text = ($"{i + 1}. {highscores[i].PlayerInitials} {highscores[i].Score}");
        }
    }

    private void LoadScores()
    {
        highscores = ScoreSaver.LoadScores();
    }

    private void TestScoresLoading()
    {
        foreach (ScoreNote scoreNote in highscores)
        {
            print(scoreNote.PlayerInitials + " " + scoreNote.Score);
        }
    }
}
