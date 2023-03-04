using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OkkaDeathState : IEnemyState
{
    private OkkaFSM _fsm;
    private bool _isDead;
    private float _timer;

    public OkkaDeathState(OkkaFSM fsm)
    {
        _fsm = fsm;
    }
    
    public void EnterState()
    {
        _isDead = false;
        _timer = 0;
        Physics2D.IgnoreCollision(_fsm.player, _fsm.col);
        
        // Stop all forces previously applied to it
        _fsm.rb.velocity = Vector2.zero;

        // Apply Knock back
        bool knockBackToRight;
        if (float.TryParse(_fsm.stateParameter, out float xDir)) {
            knockBackToRight = xDir > 0f;
        } else {
            knockBackToRight = _fsm.rb.position.x > _fsm.player.transform.position.x;
        }
        Vector2 knockBackDir = new Vector2(knockBackToRight ? 1f : -1f, 0.75f);
        _fsm.ApplyForce(knockBackDir, _fsm.enemyData.knockBackOnTakingDamageForce * 1.75f);

        _fsm.GFX.PlayDeathEffect(_fsm.enemyData.dieTime);
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _fsm.enemyData.dieTime && !_isDead) {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;

        // Check if Player's Weapon is lodged in this enemy
        CheckAndUnlodgeWeapon();

        // Drop loots
        // if (_fsm.TryGetComponent<EnemyDrop>(out EnemyDrop drop)) {
        //     drop.SpawnDrops();
        // }
        _fsm.enemyData.SpawnDrops(_fsm.transform.position);

        SaveManager.Instance.UpdateSpawnTimestamp(_fsm.gameObject.name, Time.time + _fsm.enemyData.spawnCooldown);

        GameObject.Destroy(_fsm.gameObject);
    }

    private void CheckAndUnlodgeWeapon()
    {   
        // Create a copy of children list
        List<Transform> children = new List<Transform>();
        foreach (Transform child in _fsm.transform)
        {
            children.Add(child);
        }

        foreach (Transform child in children)
        {
            if (child.TryGetComponent<WakizashiFSM>(out WakizashiFSM weapon)) {
                weapon.UnlodgedFromEnemy();
                break;
            }
        }
    }

    public void FixedUpdate() {}
    public void OnCollisionEnter2D(Collision2D collision) {}
    public void OnCollisionStay2D(Collision2D collision) {}
    public void OnCollisionExit2D(Collision2D collision) {}
    public void ExitState() {}
}
