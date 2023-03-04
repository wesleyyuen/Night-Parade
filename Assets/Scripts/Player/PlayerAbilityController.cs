using System;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class PlayerAbilityController : MonoBehaviour
{
    [Serializable]
    private struct AbilityAccess
    {
        public Ability ability;
        public MonoBehaviour component;
        public bool hasAccess;

        public AbilityAccess(Ability _ability, MonoBehaviour _component, bool _access)
        {
            ability = _ability;
            component = _component;
            hasAccess = _access;

            component.enabled = hasAccess;
        }
    }

    // New ability scripts in child of player
    // This controller manages their activations
    public enum Ability {
        All = -1,
        Jump,
        WallSlide,
        WallJump,
        Dash,
        Rewind
    }
    public float currStamina { get; private set; }
    public float maxStamina { get; private set; }
    // private float _deltaTime;
    // private bool _isStopUpdatingStamina;
    [SerializeField] private List<AbilityAccess> _abilityAccess = new List<AbilityAccess>();

    private void Start()
    {
        currStamina = 0;
        maxStamina = 0;

        // _isStopUpdatingStamina = !Constant.HAS_STAMINA;
        
        EnableAbility(Ability.All, true);
    }

    // private void Update()
    // {
    //     _deltaTime = Time.deltaTime;

    //     // Update Stamina UI
    //     if (!_isStopUpdatingStamina)
    //         StaminaUI.Instance.UpdateStaminaUI(currStamina / maxStamina);
    // }

    // public void StopUpdatingStamina()
    // {
    //     _isStopUpdatingStamina = true;
    // }

    public void EnableAbility(Ability ability, bool enable, float time = 0)
    {
        if (ability == Ability.All) {
            if (time == 0f) {
                foreach (AbilityAccess a in _abilityAccess) {
                    a.component.enabled = enable && a.hasAccess;
                }
            } else {
                foreach (AbilityAccess a in _abilityAccess) {
                    Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => a.component.enabled = e, time, enable && a.hasAccess, !enable && a.hasAccess));
                }
            }
        } else {
            AbilityAccess access = _abilityAccess[(int)ability];
            if (time == 0f) {
                access.component.enabled = enable && access.hasAccess;
            } else {
                Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => access.component.enabled = e, time, enable && access.hasAccess, !enable && access.hasAccess));
            }
        }
    }

    public void EnableAbilityExcept(Ability ability, bool enable, float time = 0)
    {
        if (time == 0f) {
            foreach (AbilityAccess a in _abilityAccess) {
                if (a.ability == ability) return;
                a.component.enabled = enable && a.hasAccess;
            }
        } else {
            foreach (AbilityAccess a in _abilityAccess) {
                if (a.ability == ability) return;
                Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => a.component.enabled = e, time, enable && a.hasAccess, !(enable && a.hasAccess)));
            }
        }
    }

    public void RegenerateStamina()
    {
        // if (Constant.HAS_STAMINA && !_isStopUpdatingStamina)
        //     currStamina = Mathf.Min(currStamina + _deltaTime, maxStamina);
    }

    public void UseStamina()
    {
        // if (Constant.HAS_STAMINA && !_isStopUpdatingStamina)
        //     currStamina = Mathf.Max(currStamina - _deltaTime, 0);
    }
}