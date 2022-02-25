using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;

    private readonly string coinText = "Coins: ";

    private int scores;

    public void UpdateScore()
    {
        scores++;
        scoreText.text = coinText + scores;
    }

    private void SaveScore()
    {
        //TODO: Save stats
    }
}
