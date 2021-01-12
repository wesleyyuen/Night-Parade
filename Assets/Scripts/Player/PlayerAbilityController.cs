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
        Dash
    }

    private PlayerJump jump;
    private PlayerWallSlide playerWallSlide;
    // private Dash dash;


    private void Awake() {
        jump = GetComponentInChildren<PlayerJump>();
        playerWallSlide = GetComponentInChildren<PlayerWallSlide>();
    }


    public void EnableAbility (Ability ability, bool enable) {
        switch (ability) {
            case Ability.Jump:
                jump.enabled = enable;
                break;

            case Ability.WallSlide:
                playerWallSlide.enabled = enable;
                break;
                
            case Ability.Dash:
                // dash.enabled = enable;
                break;

            default:
                break;
        }
    }
}
