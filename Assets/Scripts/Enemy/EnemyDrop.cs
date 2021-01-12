using UnityEngine;

public class EnemyDrop : MonoBehaviour {
    [SerializeField] private Vector2 spawnPositionOffset;
    [SerializeField] private float forceMultiplier;
    [SerializeField] private GameObject mon;
    [SerializeField] private int numOfCoinsDrop;
    [SerializeField] private float coinsDropPercent;
    [SerializeField] private GameObject heart;
    [SerializeField] private int numOfHeartsDrop;
    [SerializeField] private float heartDropPercent;
    [SerializeField] private GameObject specialDrop;
    [SerializeField] private GameObject specialDropSpawnPoint;

    public void SpawnDrops () {
        // Drop Coins
        if (Random.value > 1 - coinsDropPercent) {
            for (int i = 0; i < numOfCoinsDrop; i++) {
                GameObject coin = Instantiate (mon, gameObject.transform.position + (Vector3) spawnPositionOffset, Quaternion.identity);
                coin.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (forceMultiplier * Random.Range (-1, 1), forceMultiplier * Random.value));
            }
        }

        // Drop Health
        if (Random.value > 1 - heartDropPercent) {
            for (int i = 0; i < numOfHeartsDrop; i++) {
                GameObject health = Instantiate (heart, gameObject.transform.position + (Vector3) spawnPositionOffset, Quaternion.identity);
                health.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (forceMultiplier * Random.Range (-1, 1), forceMultiplier * Random.value));
            }
        }

        // Special Drop for Bosses, null for normal enemies
        // TODO: Add animation/late spawn via coroutine
        if (specialDrop != null) {
            Instantiate (specialDrop, specialDropSpawnPoint.transform.position + (Vector3) spawnPositionOffset, Quaternion.identity);
        }
    }
}