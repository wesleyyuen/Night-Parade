﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    // New ability scripts in child of player
    // This controller manages their activations
    public enum Ability {
        Jump,
        WallSlide,
        WallJump,
        Dash
    }
    public float currStamina { get; private set; }
    public float maxStamina { get; private set; }
    GameObject _abilitiesGB;
    PlayerJump _jump;
    PlayerWallSlide _wallSlide;
    PlayerWallJump _wallJump;
    PlayerDash _dash;
    float _deltaTime;

    void Start()
    {
        currStamina = GameMaster.Instance.savedPlayerData.MaxStamina;
        maxStamina = GameMaster.Instance.savedPlayerData.MaxStamina;

        _abilitiesGB = transform.Find("Abilities").gameObject;
        _jump = _abilitiesGB.GetComponent<PlayerJump>();
        _wallSlide = _abilitiesGB.GetComponent<PlayerWallSlide>();
        _wallJump = _abilitiesGB.GetComponent<PlayerWallJump>();
        _dash = _abilitiesGB.GetComponent<PlayerDash>();

        // Enable Abilities
        EnableAbility(Ability.Jump, true);
        EnableAbility(Ability.WallJump, false);
        EnableAbility(Ability.WallSlide, false);
        EnableAbility(Ability.Dash, false);
    }

    void Update()
    {
        _deltaTime = Time.deltaTime;

        // Update Stamina UI
        StaminaUI.Instance.UpdateStaminaUI(currStamina / maxStamina);
    }

    public void EnableAbility (Ability ability, bool enable, float time = 0)
    {
        switch (ability) {
            case Ability.Jump:
                _jump.EnablePlayerJump(enable, time);
                break;

            case Ability.WallSlide:
                _wallSlide.enabled = enable;
                break;

            case Ability.WallJump:
                _wallJump.enabled = enable;
                break;
                
            case Ability.Dash:
                _dash.enabled = enable;
                break;

            default:
                break;
        }
    }

    public void RegenerateStamina()
    {
        currStamina = Mathf.Min(currStamina + _deltaTime, maxStamina);
    }

    public void UseStamina()
    {
        currStamina = Mathf.Max(currStamina - _deltaTime, 0);
    }
}
