using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerWallSlide : MonoBehaviour
{
    private InputManager _inputManager;
    private Rigidbody2D _rb;
    private PlayerPlatformCollision _collision;
    private WeaponFSM _weaponFSM;
    [SerializeField] private float slideSpeed;
    // TODO: use event instead
    [HideInInspector] public bool canSlide;

    [Inject]
    public void Initialize(InputManager inputManager)
    {
        _inputManager = inputManager;
    }

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _collision = GetComponentInParent<PlayerPlatformCollision>();
        _weaponFSM = transform.parent.GetComponentInChildren<WeaponFSM>();

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
            if ((_collision.onLeftWall && _inputManager.HasDirectionalInput(InputManager.DirectionInput.Left))
             || (_collision.onRightWall && _inputManager.HasDirectionalInput(InputManager.DirectionInput.Right)) ) {
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
