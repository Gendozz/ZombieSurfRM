using UnityEngine;
using UnityEngine.Events;

public class GameEventListner : MonoBehaviour
{
    public GameEvent gameEvent;

    public UnityEvent response;

    private void OnEnable()
    {
        gameEvent.RegisterListner(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListner(this);
    }

    public void OnEventRaised()
    {
        response?.Invoke();
    }


}
