using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlide : MonoBehaviour
{
    Rigidbody2D _rb;
    PlayerAnimations _anim;
    PlayerPlatformCollision _collision;
    WeaponFSM _weaponFSM;
    InputMaster _input;
    [SerializeField] float slideSpeed;
    [HideInInspector] public bool canSlide;

    void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _anim = GetComponentInParent<PlayerAnimations>();
        _weaponFSM = transform.parent.GetComponentInChildren<WeaponFSM>();
        _collision = GetComponentInParent<PlayerPlatformCollision>();
        canSlide = true;

        // Handle Input
        _input = new InputMaster();
        _input.Player.Movement.Enable();
    }

    void Update()
    {
        if (!canSlide) return;

        // Only disable attack if player is wallsliding
        _weaponFSM.canAttack = true;

        // Must be falling
        if (!_collision.onGround && _collision.onWall && _rb.velocity.y < 0) {
            float xRaw = _input.Player.Movement.ReadValue<Vector2>().x;
            // Must be pressing against wall
            if ((_collision.onLeftWall && (xRaw < 0)) || (_collision.onRightWall && (xRaw > 0)) ) {
                WallSlide(slideSpeed);
            }
        }
    }

    void WallSlide(float speed)
    {
        _weaponFSM.canAttack = false;
        _rb.velocity = new Vector2(_rb.velocity.x, -speed);
    }
}
