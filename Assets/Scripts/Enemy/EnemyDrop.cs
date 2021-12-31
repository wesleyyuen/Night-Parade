using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    [SerializeField] Vector2 spawnPositionOffset;
    [SerializeField] Vector2 force;
    [SerializeField] GameObject mon;
    [SerializeField] int numOfCoinsDrop;
    [SerializeField] float coinsDropPercent;
    [SerializeField] GameObject heart;
    [SerializeField] int numOfHeartsDrop;
    [SerializeField] float heartDropPercent;
    [SerializeField] GameObject specialDrop;
    [SerializeField] GameObject specialDropSpawnPoint;

    public void SpawnDrops()
    {
        // Drop Coins
        if (Random.value > 1 - coinsDropPercent) {
            for (int i = 0; i < numOfCoinsDrop; i++) {
                GameObject coin = Instantiate(mon, gameObject.transform.position + (Vector3) spawnPositionOffset, Quaternion.identity);
                coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(force.x * Random.Range(-1, 1), force.y * Random.Range(0.5f, 1)));
            }
        }

        // Drop Health
        if (Random.value > 1 - heartDropPercent) {
            for (int i = 0; i < numOfHeartsDrop; i++) {
                GameObject health = Instantiate(heart, gameObject.transform.position + (Vector3) spawnPositionOffset, Quaternion.identity);
                health.GetComponent<Rigidbody2D>().AddForce(new Vector2(force.x * Random.Range(-1, 1), force.y * Random.Range(0.5f, 1)));
            }
        }

        // Special Drop for Bosses, null for normal enemies
        if (specialDrop != null) {
            Instantiate (specialDrop, specialDropSpawnPoint.transform.position + (Vector3) spawnPositionOffset, Quaternion.identity);
        }
    }
}