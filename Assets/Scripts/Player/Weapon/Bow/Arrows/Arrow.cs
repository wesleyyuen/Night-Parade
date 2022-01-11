using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private bool _hasCollided;

    private void Awake()
    {
        _hasCollided = false;
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }
    
    public virtual void Shoot(Vector2 dir)
    {
        _rb.velocity = dir.normalized * _speed;
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasCollided) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            other.gameObject.layer == LayerMask.NameToLayer("Wall") ||
            other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            HandleHit(other.gameObject);
    }

    private void HandleHit(GameObject hit)
    {
        _hasCollided = true;
        
        // Damage Enemy
        if (hit.TryGetComponent<EnemyFSM>(out EnemyFSM enemy)) {
            enemy.TakeDamage(_damage, _rb.velocity);
        }

        transform.parent = hit.gameObject.transform;
        _rb.velocity = Vector2.zero;
        _collider.enabled = false;

        Destroy(gameObject, 3f);
    }
}
