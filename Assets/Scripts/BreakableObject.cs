using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] string keyString;
    [SerializeField] int numOfHitsToDestroy;
    public enum BreakableSide {
        left,
        right,
        both
    }

    [SerializeField] BreakableSide side;
    [HideInInspector] public int currentHealth {get; protected set;}

    protected virtual void Start()
    {
        if (keyString == "") {
            Debug.LogError("Error: Key String for Breakable Object " + gameObject.name + " is empty!");
        }

        // Do not spawn if player destroyed previously
        if (SaveManager.Instance.HasScenePermaProgress(GameMaster.Instance.currentScene, keyString))
            Destroy(gameObject);

        currentHealth = numOfHitsToDestroy;
    }

    public virtual void TakeDamage(bool fromLeft)
    {
        if (side == BreakableSide.both || // attack from either side is fine
            (fromLeft && side == BreakableSide.left) || // attack from left
            (!fromLeft && side == BreakableSide.right)) { // attack from right
            currentHealth--;
            if (currentHealth <= 0) {
                Break();
            }
        } else {
            // TODO: Play weapon clink effect
        }
    }

    protected void Break()
    {
        SaveManager.Instance.AddScenePermaProgress(GameMaster.Instance.currentScene, keyString, 1);
        Destroy(gameObject);
    }
}