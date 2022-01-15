using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class WakizashiBlockState : IWeaponState, IBindInput
{
    private WakizashiFSM _fsm;
    private PlayerMovement _playerMovement;
    private PlayerAnimations _playerAnimation;
    private PlayerAbilityController _abilityController;
    private bool _wasBlocking;
    private bool _isBlockReleasedBeforeMinDuration;
    private bool _hasNextAttack, _hasNextThrow;
    private bool _stopUpdating;

    public WakizashiBlockState(WakizashiFSM fsm)
    {
        _fsm = fsm;
        _abilityController = fsm.GetComponentInParent<PlayerAbilityController>();
        _playerMovement = fsm.GetComponentInParent<PlayerMovement>();
        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
    }

    public void BindInput()
    {
        _fsm.InputActions.Player.Block.canceled += OnReleaseBlock;
        _fsm.InputActions.Player.Attack.started += OnNextAttack;
        _fsm.InputActions.Player.Throw_SlowTap.started += OnNextThrow;
    }

    public void UnbindInput()
    {
        _fsm.InputActions.Player.Block.canceled -= OnReleaseBlock;
        _fsm.InputActions.Player.Attack.started -= OnNextAttack;
        _fsm.InputActions.Player.Throw_SlowTap.started -= OnNextThrow;
    }

    public void EnterState()
    {
        _isBlockReleasedBeforeMinDuration = false;
        _hasNextAttack = false;
        _hasNextThrow = false;
        _wasBlocking = false;
        _stopUpdating = false;
        
        _playerAnimation.SetBlockAnimation(true);
        _playerAnimation.EnablePlayerTurning(false);
        _playerMovement.EnablePlayerMovement(false);
    }

    private void OnReleaseBlock(InputAction.CallbackContext context)
    {
        if (!_isBlockReleasedBeforeMinDuration && _fsm.currentBlockTimer >= _fsm.weaponData.blockMinDuration)
            _fsm.SetState(_fsm.states[WakizashiStateType.Idle]);
    }

    private void OnNextAttack(InputAction.CallbackContext context)
    {
        _hasNextAttack = true;
    }

    private void OnNextThrow(InputAction.CallbackContext context)
    {
        _hasNextThrow = true;
    }

    public void Update()
    {
        if (_stopUpdating) return;

        _fsm.currentBlockTimer += Time.deltaTime;
        _abilityController.UseStamina();

        float xScale = _playerAnimation.IsFacingRight() ? 1f : -1f;

        // Handle Release earlier than InputActions' minDuration
        if (_fsm.InputActions.Player.Block.ReadValue<float>() <= 0.5f && !_isBlockReleasedBeforeMinDuration && _fsm.currentBlockTimer < _fsm.weaponData.blockMinDuration) {
            _isBlockReleasedBeforeMinDuration = true;
        }


        if (_abilityController.currStamina <= 0) {
            _stopUpdating = true;
            _fsm.SetState(_fsm.states[WakizashiStateType.Idle]);
            return;
        }

        if (_isBlockReleasedBeforeMinDuration && _fsm.currentBlockTimer >= _fsm.weaponData.blockMinDuration) {
            _stopUpdating = true;
            if (_hasNextAttack) {
                _fsm.SetState(_fsm.states[WakizashiStateType.Attack]);
            }
            else if (_hasNextThrow) {
                _fsm.SetState(_fsm.states[WakizashiStateType.Throw]);
            }
            else {
                _fsm.SetState(_fsm.states[WakizashiStateType.Idle]);
            }
            return;
        }

        Vector2 blockPoint = (Vector2)_fsm.player.transform.TransformPoint(new Vector3(xScale * _fsm.weaponData.blockPoint.x, _fsm.weaponData.blockPoint.y));
        Collider2D[] blocked = Physics2D.OverlapAreaAll(blockPoint + new Vector2(-_fsm.weaponData.blockRange.x/2, _fsm.weaponData.blockRange.y/2),
                                                        blockPoint + new Vector2(_fsm.weaponData.blockRange.x/2, -_fsm.weaponData.blockRange.y/2),
                                                        LayerMask.GetMask("Enemies"));
        _fsm.hasBlocked = false;
        if (blocked.Length == 0) {
            _wasBlocking = _fsm.hasBlocked;
            return;
        } else {
            _fsm.hasBlocked = true;
        }

        foreach (Collider2D hit in blocked) {
            EnemyFSM enemy = hit.GetComponent<EnemyFSM>();
            if (enemy != null && !enemy.IsDead()) {
                Vector2 dir = new Vector2(xScale, 0f);
                enemy.ApplyForce(dir, enemy.enemyData.knockBackOnBlockedForce, enemy.enemyData.timeStunnedAfterBlocked);
                enemy.StunForSeconds(enemy.enemyData.timeStunnedAfterBlocked);
            }
        }

        if (!_wasBlocking && _fsm.hasBlocked) {
            Vector2 dir = new Vector2(xScale, 0f);
            _playerMovement.ApplyKnockback(-dir , _fsm.weaponData.blockKnockback, 0.05f);
        }

        _wasBlocking = _fsm.hasBlocked;
    }

    public void FixedUpdate()
    {
    }

    public void ExitState()
    {
        _playerAnimation.SetBlockAnimation(false);
        _playerAnimation.EnablePlayerTurning(true);
        _playerMovement.EnablePlayerMovement(true);
    }
}
