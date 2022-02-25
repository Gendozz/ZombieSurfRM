using System.Timers;

public class DifficultyChanger
{
    private FloatReference currentDifficulty;

    private float difficultyIncrement;

    private double changeDifficultyPeriod;

    private Timer timer;

    public DifficultyChanger(FloatReference difficulty, FloatReference difficultyIncrement, FloatReference changeDifficultyPeriod)
    {
        currentDifficulty = difficulty;
        this.difficultyIncrement = difficultyIncrement.GetValue();
        this.changeDifficultyPeriod = changeDifficultyPeriod.GetValue();
    }

    public void StartChangeDifficulty()
    {        
        timer = new Timer(changeDifficultyPeriod);

        timer.Elapsed += ChangeDifficulty;
        timer.AutoReset = true;
        timer.Enabled = true;
    }

    private void ChangeDifficulty(System.Object source, ElapsedEventArgs e)
    {
        currentDifficulty.value += difficultyIncrement;
    }

    public void StopChangeDifficulty()
    {
        timer.Stop();
    }

}


