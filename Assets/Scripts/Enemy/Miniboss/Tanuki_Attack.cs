using UnityEngine;

public class Tanuki_Attack : MonoBehaviour {
    [SerializeField] private Vector3 attackOffset;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask attackMask;

    // NOT IN USE RIGHT NOW
    public void Attack () {

        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D[] colInfo = Physics2D.OverlapCircleAll (pos, attackRange, attackMask);

        if (colInfo.Length == 0) return;

        foreach (Collider2D enemy in colInfo) {
            if (enemy.name == "Player") enemy.GetComponent<PlayerHealth> ().TakeDamage (4, transform.position);
        }
    }

    void OnDrawGizmosSelected () {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere (pos, attackRange);
    }
}