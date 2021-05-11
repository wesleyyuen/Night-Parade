using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    // New ability scripts in child of player
    // This controller manages their activations
    [SerializeField] private GameObject abilitiesGB;

    public enum Ability {
        Jump,
        WallSlide,
        WallJump,
        Dash
    }

    private PlayerJump jump;
    private PlayerWallSlide wallSlide;
    private PlayerWallJump wallJump;
    private PlayerDash dash;


    private void Awake() {
        jump = abilitiesGB.GetComponent<PlayerJump>();
        wallSlide = abilitiesGB.GetComponent<PlayerWallSlide>();
        wallJump = abilitiesGB.GetComponent<PlayerWallJump>();
        dash = abilitiesGB.GetComponent<PlayerDash>();

        // Enable Abilities
        EnableAbility(Ability.Jump, true);
        EnableAbility(Ability.WallJump, true);
        EnableAbility(Ability.WallSlide, true);
        EnableAbility(Ability.Dash, true);
    }


    public void EnableAbility (Ability ability, bool enable, float time = 0) {
        switch (ability) {
            case Ability.Jump:
                jump.EnablePlayerJump(enable, time);
                break;

            case Ability.WallSlide:
                wallSlide.enabled = enable;
                break;

            case Ability.WallJump:
                wallJump.enabled = enable;
                break;
                
            case Ability.Dash:
                dash.enabled = enable;
                break;

            default:
                break;
        }
    }
}
