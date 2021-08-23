using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public string keyString;
    public int numOfHitsToDestroy;
    public enum breakableSide {
        left,
        right,
        both
    }

    public breakableSide side;
    [HideInInspector] public int currentHealth;

    public virtual void Start ()
    {
        if (keyString == "") {
            Debug.LogError("Error: Key String for Breakable Object " + gameObject.name + " is empty!");
        }
        // Do not spawn if player destroyed previously
        bool destroyedBefore;
        FindObjectOfType<PlayerProgress> ().areaProgress.TryGetValue (keyString, out destroyedBefore);
        if (destroyedBefore) Destroy (gameObject);

        currentHealth = numOfHitsToDestroy;
    }

    public virtual void TakeDamage (bool fromLeft)
    {
        if (side == breakableSide.both || // attack from either side is fine
            (fromLeft && side == breakableSide.left) || // attack from left
            (!fromLeft && side == breakableSide.right)) { // attack from right
            currentHealth--;
            if (currentHealth <= 0) {
                Break ();
            }
        } else {
            // Play blade clink sword effect
        }
    }

    public void Break ()
    {
        FindObjectOfType<PlayerProgress> ().areaProgress.Add (keyString, true);
        Destroy (gameObject);
    }
}