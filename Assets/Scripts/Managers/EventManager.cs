using System;

public interface IEventManager
{
    public event Action<int, int, int, float> Event_PlayerDamaged;
    void OnPlayerDamaged(int prev, int curr, int max, float duration = 0.75f);
}

public class EventManager : IEventManager
{
    public event Action<int, int, int, float> Event_PlayerDamaged;
    public void OnPlayerDamaged(int prev, int curr, int max, float duration = 0.75f)
    {
        Event_PlayerDamaged?.Invoke(prev, curr, max, duration);
    }
}
