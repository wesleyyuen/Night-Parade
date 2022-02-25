using UnityEngine;
using UnityEngine.Events;

public class UnityGameEventListener : MonoBehaviour, IGameEventListener
{
    [SerializeField] private GameEvent _event;
    [SerializeField] private UnityEvent _response;

    public void RegisterListener()
    {
        _event?.RegisterListener(this);
    }

    public void UnregisterListener()
    {
        _event?.UnregisterListener(this);
    }

    public void OnEventInvoked()
    {
        _response?.Invoke();
    }
}