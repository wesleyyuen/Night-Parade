using UnityEngine;

public class EnemyDrop : MonoBehaviour {
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
                GameObject coin = Instantiate (mon, gameObject.transform.position, Quaternion.identity);
                coin.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (forceMultiplier * Random.Range (-1, 1), forceMultiplier * Random.value));
            }
        }

        // Drop Health
        if (Random.value > 1 - heartDropPercent) {
            for (int i = 0; i < numOfHeartsDrop; i++) {
                GameObject health = Instantiate (heart, gameObject.transform.position, Quaternion.identity);
                health.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (forceMultiplier * Random.Range (-1, 1), forceMultiplier * Random.value));
            }
        }

        // Special Drop for Bosses, null for normal enemies
        if (specialDrop != null) {
            Instantiate (specialDrop, specialDropSpawnPoint.transform.position, Quaternion.identity);
        }
    }
}