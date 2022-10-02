using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Collections.Generic;

public static class ScoreSaver
{
    static readonly string fileName = "/scores2.sav";

    public static void SaveScores(List<ScoreNote> highscores)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + fileName);

        binaryFormatter.Serialize(file, highscores);
        file.Close();
    }


    public static List<ScoreNote> LoadScores()
    {
        List<ScoreNote> scoreNotes = new List<ScoreNote>();
        if(File.Exists(Application.persistentDataPath + fileName))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);
            scoreNotes = binaryFormatter.Deserialize(file) as List<ScoreNote>;
            file.Close();

            if(scoreNotes == null)
            {
                Debug.Log("There's no saved data");
            }
        }
        else
        {
            Debug.LogError("There's no saved file");
        }

        return scoreNotes;
    }
}
