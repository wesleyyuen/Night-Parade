using UnityEngine;

public class BreakableObject : MonoBehaviour {
    public string keyString;

    public int numOfHitsToDestroy;
    public enum breakableSide {
        left,
        right,
        both
    }

    public breakableSide side;
    [HideInInspector] public int currentHealth;

    public virtual void Start () {
        // TODO: this pattern of DestroyedBefore maybe better if abstracted to own script
        bool destroyedBefore;
        FindObjectOfType<PlayerProgress> ().areaProgress.TryGetValue (keyString, out destroyedBefore);
        if (destroyedBefore) Destroy (gameObject);

        currentHealth = numOfHitsToDestroy;
    }

    public virtual void TakeDamage (GameObject player) {
        float dir = player.transform.position.x - gameObject.transform.position.x;
        if (side == breakableSide.both || // attack from either side is fine
            (dir < 0 && side == breakableSide.left) || // attack from left
            (dir > 0 && side == breakableSide.right)) { // attack from right
            currentHealth--;
            if (currentHealth <= 0) {
                Break ();
            }
        } else {
            // Play blade clink sword effect
        }
    }

    public void Break () {
        FindObjectOfType<PlayerProgress> ().areaProgress.Add (keyString, true);
        Destroy (gameObject);
    }
}