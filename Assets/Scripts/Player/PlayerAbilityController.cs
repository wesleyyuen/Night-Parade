using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class PlayerAbilityController : MonoBehaviour
{
    // New ability scripts in child of player
    // This controller manages their activations
    public enum Ability {
        All,
        Jump,
        WallSlide,
        WallJump,
        Dash
    }
    public float currStamina { get; private set; }
    public float maxStamina { get; private set; }
    private GameObject _abilitiesGB;
    private PlayerJump _jump;
    private PlayerWallSlide _wallSlide;
    private PlayerWallJump _wallJump;
    private PlayerDash _dash;
    private float _deltaTime;
    private bool _isStopUpdatingStamina;

    private void Start()
    {
        currStamina = 0;
        maxStamina = 0;

        _isStopUpdatingStamina = !Constant.HAS_STAMINA;

        _abilitiesGB = transform.Find("Abilities").gameObject;
        _jump = _abilitiesGB.GetComponent<PlayerJump>();
        _wallSlide = _abilitiesGB.GetComponent<PlayerWallSlide>();
        _wallJump = _abilitiesGB.GetComponent<PlayerWallJump>();
        _dash = _abilitiesGB.GetComponent<PlayerDash>();

        // Enable Abilities
        EnableAbility(Ability.Jump, true);
        EnableAbility(Ability.WallJump, false);
        EnableAbility(Ability.WallSlide, false);
        EnableAbility(Ability.Dash, true);
    }

    private void Update()
    {
        _deltaTime = Time.deltaTime;

        // Update Stamina UI
        // if (!_isStopUpdatingStamina)
        //     StaminaUI.Instance.UpdateStaminaUI(currStamina / maxStamina);
    }

    public void StopUpdatingStamina()
    {
        _isStopUpdatingStamina = true;
    }

    public void EnableAbility(Ability ability, bool enable, float time = 0)
    {
        switch (ability) {
            case Ability.All:
                if (time == 0f) {
                    _jump.enabled = enable;
                    _wallSlide.enabled = enable;
                    _wallJump.enabled = enable;
                    _dash.enabled = enable;
                } else {
                    Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _jump.enabled = e, time, enable, !enable));
                    Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _wallSlide.enabled = e, time, enable, !enable));
                    Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _wallJump.enabled = e, time, enable, !enable));
                    Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _dash.enabled = e, time, enable, !enable));
                }
                break;

            case Ability.Jump:
                if (time == 0f) {
                    _jump.enabled = enable;
                } else {
                    Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _jump.enabled = e, time, enable, !enable));
                }
                break;

            case Ability.WallSlide:
                if (time == 0f) {
                    _wallSlide.enabled = enable;
                } else {
                    Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _wallSlide.enabled = e, time, enable, !enable));
                }
                break;

            case Ability.WallJump:
                if (time == 0f) {
                    _wallJump.enabled = enable;
                } else {
                    Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _wallJump.enabled = e, time, enable, !enable));
                }
                break;
                
            case Ability.Dash:
                if (time == 0f) {
                    _dash.enabled = enable;
                } else {
                    Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _dash.enabled = e, time, enable, !enable));
                }
                break;

            default:
                break;
        }
    }

    public void RegenerateStamina()
    {
        if (Constant.HAS_STAMINA && !_isStopUpdatingStamina)
            currStamina = Mathf.Min(currStamina + _deltaTime, maxStamina);
    }

    public void UseStamina()
    {
        if (Constant.HAS_STAMINA && !_isStopUpdatingStamina)
            currStamina = Mathf.Max(currStamina - _deltaTime, 0);
    }
}
