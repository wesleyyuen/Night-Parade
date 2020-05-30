using UnityEngine;

public class EnemyDrop : MonoBehaviour {
    public GameObject mon;
    public int numOfCoinsDrop;
    public float coinsDropPercent;
    public GameObject heart;
    public int numOfHeartsDrop;
    public float heartDropPercent;
    public void SpawnDrops () {
        if (Random.value > 1 - coinsDropPercent) {
            for (int i = 0; i < numOfCoinsDrop; i++) {
                Instantiate (mon, gameObject.transform.position, Quaternion.identity);
            }
        }
        if (Random.value > 1 - heartDropPercent) {
            for (int i = 0; i < numOfHeartsDrop; i++) {
                Instantiate (heart, gameObject.transform.position, Quaternion.identity);
            }
        }
    }
}