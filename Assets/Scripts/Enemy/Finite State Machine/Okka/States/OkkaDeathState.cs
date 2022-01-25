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
        _fsm.rb.angularVelocity = 0f;

        // Apply Knock back
        float knockBackForce = _fsm.enemyData.knockBackOnTakingDamageForce * 3f;
        Vector2 knockBackDir = new Vector2((_fsm.rb.position.x > _fsm.player.transform.position.x) ? 1f : -1f, 0.75f);
        _fsm.ApplyForce(knockBackDir, knockBackForce);

        // Check if Player's Weapon is lodged in this enemy
        CheckAndUnlodgeWeapon();

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

        // Drop loots
        if (_fsm.TryGetComponent<EnemyDrop>(out EnemyDrop drop)) {
            drop.SpawnDrops();
        }

        SaveManager.Instance.UpdateSpawnTimestamp(_fsm.gameObject.name, Time.time + _fsm.enemyData.spawnCooldown);

        GameObject.Destroy(_fsm.gameObject);
    }

    private void CheckAndUnlodgeWeapon()
    {
        Debug.Log(GameObject.FindGameObjectWithTag("Weapon").transform.parent?.name);
        
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
