using System;
using UnityEngine;

// public interface EventManager
// {
// #region UI
//     public event Action Event_OnUIIntro;
//     void OnUIIntro();
//     public event Action Event_OnUIOutro;
//     void OnUIOutro();
// #endregion

// #region Player
//     public event Action<int, int, int, float> Event_OnPlayerDamaged;
//     void OnPlayerDamaged(int prev, int curr, int max, float duration = 0.75f);

//     public event Action<int, int, int, float> Event_OnPlayerHealed;
//     void OnPlayerHealed(int prev, int curr, int max, float duration = 0.75f);

//     public event Action<int, int> Event_OnPlayerMonIncreased;
//     void OnPlayerMonIncreased(int prev, int curr);

//     public event Action Event_OnPlayerGroundEntered;
//     void OnPlayerGroundEntered();
// #endregion
// }

public class EventManager
{
#region UI
    public event Action Event_OnUIIntro;
    public void OnUIIntro()
    {
        Event_OnUIIntro?.Invoke();
    }

    public event Action Event_OnUIOutro;
    public void OnUIOutro()
    {
        Event_OnUIOutro?.Invoke();
    }
#endregion

#region Player
    public event Action<int, int, int, float> Event_OnPlayerDamaged;
    public void OnPlayerDamaged(int prev, int curr, int max, float duration = 0.75f)
    {
        Event_OnPlayerDamaged?.Invoke(prev, curr, max, duration);
    }

    public event Action<int, int, int, float> Event_OnPlayerHealed;
    public void OnPlayerHealed(int prev, int curr, int max, float duration = 0.75f)
    {
        Event_OnPlayerHealed?.Invoke(prev, curr, max, duration);
    }

    public event Action<int, int> Event_OnPlayerMonIncreased;
    public void OnPlayerMonIncreased(int prev, int curr)
    {
        Event_OnPlayerMonIncreased?.Invoke(prev, curr);
    }

    public event Action Event_OnPlayerGroundEntered;
    public void OnPlayerGroundEntered()
    {
        Event_OnPlayerGroundEntered?.Invoke();
    }
#endregion
}
