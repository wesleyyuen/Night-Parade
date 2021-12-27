using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlide : MonoBehaviour
{
    Rigidbody2D _rb;
    PlayerAnimations _anim;
    PlayerPlatformCollision _collision;
    WeaponFSM _weaponFSM;
    [SerializeField] float slideSpeed;
    [HideInInspector] public bool canSlide;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _anim = GetComponentInParent<PlayerAnimations>();
        _weaponFSM = transform.parent.GetComponentInChildren<WeaponFSM>();
        _collision = GetComponentInParent<PlayerPlatformCollision>();
        canSlide = true;
    }

    private void Update()
    {
        if (!canSlide) return;

        // Only disable attack if player is wallsliding
        _weaponFSM.canAttack = true;

        // Must be falling
        if (!_collision.onGround && _collision.onWall && _rb.velocity.y < 0) {
            // Must be pressing against wall
            if ((_collision.onLeftWall && InputManager.Instance.HasDirectionalInput(InputManager.DirectionInput.Left))
             || (_collision.onRightWall && InputManager.Instance.HasDirectionalInput(InputManager.DirectionInput.Right)) ) {
                WallSlide(slideSpeed);
            }
        }
    }

    private void WallSlide(float speed)
    {
        _weaponFSM.canAttack = false;
        _rb.velocity = new Vector2(_rb.velocity.x, -speed);
    }
}
