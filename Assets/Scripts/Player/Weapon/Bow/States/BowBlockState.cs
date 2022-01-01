using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class BowBlockState : IWeaponState, IBindInput
{
    BowFSM _fsm;
    PlayerMovement _playerMovement;
    PlayerAnimations _playerAnimation;
    PlayerAbilityController _abilityController;
    bool _wasBlocking;
    bool _isBlockReleasedBeforeMinDuration;
    bool _hasNextAttack;

    public BowBlockState(BowFSM fsm)
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
    }

    public void UnbindInput()
    {
        _fsm.InputActions.Player.Block.canceled -= OnReleaseBlock;
        _fsm.InputActions.Player.Attack.started -= OnNextAttack;
    }

    public void EnterState()
    {
        _isBlockReleasedBeforeMinDuration = false;
        _hasNextAttack = false;
        _wasBlocking = false;
        
        _playerAnimation.SetBlockAnimation(true);
        _playerAnimation.EnablePlayerTurning(false);
        _playerMovement.EnablePlayerMovement(false);
    }

    private void OnReleaseBlock(InputAction.CallbackContext context)
    {
        if (!_isBlockReleasedBeforeMinDuration && _fsm.currentBlockTimer >= _fsm.weaponData.blockMinDuration)
            _fsm.SetState(_fsm.states[BowStateType.Idle]);
    }

    private void OnNextAttack(InputAction.CallbackContext context)
    {
        _hasNextAttack = true;
        if (!_isBlockReleasedBeforeMinDuration && _fsm.currentBlockTimer >= _fsm.weaponData.blockMinDuration)
            _fsm.SetState(_fsm.states[BowStateType.Attack]);
    }

    public void Update()
    {
        _fsm.currentBlockTimer += Time.deltaTime;
        _abilityController.UseStamina();

        float xScale = _playerAnimation.IsFacingRight() ? 1f : -1f;

        if (_fsm.InputActions.Player.Block.ReadValue<float>() <= 0.5f && !_isBlockReleasedBeforeMinDuration && _fsm.currentBlockTimer < _fsm.weaponData.blockMinDuration) {
            _isBlockReleasedBeforeMinDuration = true;
        }

        if (_hasNextAttack && _isBlockReleasedBeforeMinDuration && _fsm.currentBlockTimer >= _fsm.weaponData.blockMinDuration) {
            _fsm.SetState(_fsm.states[BowStateType.Attack]);
        }
        else if (_abilityController.currStamina <= 0
            || (_isBlockReleasedBeforeMinDuration && _fsm.currentBlockTimer >= _fsm.weaponData.blockMinDuration)) {
            _fsm.SetState(_fsm.states[BowStateType.Idle]);
        }

        Vector2 blockPoint = (Vector2)_fsm.player.transform.TransformPoint(new Vector3(xScale * _fsm.weaponData.blockPoint.x, _fsm.weaponData.blockPoint.y));
        Collider2D[] blocked = Physics2D.OverlapAreaAll(blockPoint + new Vector2(-_fsm.weaponData.blockRange.x/2, _fsm.weaponData.blockRange.y/2),
                                                        blockPoint + new Vector2(_fsm.weaponData.blockRange.x/2, -_fsm.weaponData.blockRange.y/2),
                                                        LayerMask.GetMask("Enemies"));
        _fsm.hasBlocked = false;
        if (blocked.Length == 0) {
            _wasBlocking = _fsm.hasBlocked;
            return;
        } else
            _fsm.hasBlocked = true;

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
