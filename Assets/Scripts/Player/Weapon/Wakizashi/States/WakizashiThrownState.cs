using UnityEngine;
using UnityEngine.InputSystem;

public sealed class WakizashiThrownState : IWeaponState
{
    WakizashiFSM _fsm;
    Rigidbody2D _rb;
    PlayerAnimations _playerAnimation;
    int _layerMasks;

    public WakizashiThrownState(WakizashiFSM fsm)
    {
        _fsm = fsm;
        _rb = fsm.GetComponent<Rigidbody2D>();
        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
        _layerMasks = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Wall")) | (1 << LayerMask.NameToLayer("Enemies"));
    }
 
    public void EnterState()
    {
        // play animation
        // TODO: should be called from player animation
        _playerAnimation.SetThrowAnimation();
        Throw();
    }

    public void Update()
    {
        // Rotate Wakizashi
        // TODO: replace by animation (rotate in x/y axis mostly)
        _rb.transform.localEulerAngles += (_playerAnimation.IsFacingRight() ? Vector3.back : Vector3.forward) * 1800f * Time.deltaTime; 
    }

    public void FixedUpdate()
    {
        // Check Collision
        Collider2D hit = Physics2D.OverlapArea(_fsm.transform.position - new Vector3(0.5f, 0.5f),
                                               _fsm.transform.position + new Vector3(0.5f, 0.5f),
                                               _layerMasks);
                                               
        if (hit) {
            HandleHit(hit.gameObject);
        }
    }

    void Throw()
    {
        _rb.isKinematic = false;

        // Detach from player
        _rb.transform.parent = null;

        _rb.AddForce((_playerAnimation.IsFacingRight() ? Vector2.right : Vector2.left) * _fsm.weaponData.throwForce, ForceMode2D.Impulse);
    }

    void HandleHit(GameObject hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
            hit.gameObject.GetComponent<EnemyFSM>().TakeDamage(_fsm.weaponData.throwDamage);
        }
        _rb.transform.parent = hit.gameObject.transform;
        _fsm.SetState(_fsm.states[WeaponFSM.StateType.LodgedState]);
    }

    public void ExitState()
    {
        _rb.velocity = Vector2.zero;
        _rb.isKinematic = true;
    }
}
