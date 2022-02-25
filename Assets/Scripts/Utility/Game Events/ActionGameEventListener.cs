using UnityEngine;

public class ActionGameEventListener : MonoBehaviour, IGameEventListener
{
    [SerializeField] private GameEvent _event;
    private event System.Action _response;

    public void RegisterListener(System.Action response)
    {
        _event?.RegisterListener(this);
        _response = response;
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
