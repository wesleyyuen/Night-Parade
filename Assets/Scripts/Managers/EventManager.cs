using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EventManager", menuName = "ScriptableObjects/Singletons/Managers/EventManager")]
public class EventManager : ScriptableSingleton<EventManager>
{
    public event Action<int, int, int, float> Event_PlayerDamaged;
    public void OnPlayerDamaged(int prev, int curr, int max, float duration = 0.75f)
    {
        Event_PlayerDamaged?.Invoke(prev, curr, max, duration);
    }
}
