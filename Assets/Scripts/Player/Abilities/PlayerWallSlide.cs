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

    void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _anim = GetComponentInParent<PlayerAnimations>();
        _weaponFSM = transform.parent.GetComponentInChildren<WeaponFSM>();
        _collision = GetComponentInParent<PlayerPlatformCollision>();
        canSlide = true;
    }

    void Update()
    {
        if (!canSlide) return;

        // Only disable attack if player is wallsliding
        _weaponFSM.canAttack = true;

        if (!_collision.onGround && _collision.onWall) {
            // press against wall, slow down sliding
            if ((_collision.onLeftWall && (Input.GetAxisRaw("Horizontal") < 0)) ||
                (_collision.onRightWall && (Input.GetAxisRaw("Horizontal") > 0))) {
                _weaponFSM.canAttack = false;
                WallSlide(slideSpeed);
            }
        }
    }

    void WallSlide(float speed)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, -speed);
    }
}
