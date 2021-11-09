using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newGameEvent")]
public class GameEvent : ScriptableObject
{
    private readonly List<GameEventListner> eventListners = new List<GameEventListner>();

    public void Raise()
    {
        for (int i = eventListners.Count - 1; i >= 0; i--)
        {
            eventListners[i].OnEventRaised();
        }
    }

    public void RegisterListner(GameEventListner listner)
    {
        if (!eventListners.Contains(listner))
        {
            eventListners.Add(listner);
        }
    }

    public void UnregisterListner(GameEventListner listner)
    {
        if (eventListners.Contains(listner))
        {
            eventListners.Remove(listner);
        }
    }
}
