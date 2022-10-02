using System;

[Serializable]
public class ScoreNote : IComparable<ScoreNote>
{
    public string PlayerInitials { get; private set; }

    public int Score { get; private set; }

    public ScoreNote(string playerInitials, int score)
    {
        PlayerInitials = playerInitials;
        Score = score;
    }

    public int CompareTo(ScoreNote compareScoreNote)
    {
        if (compareScoreNote == null)
            return 1;

        else
            return this.Score.CompareTo(compareScoreNote.Score);
    }
}
