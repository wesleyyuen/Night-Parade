using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int currHealth { get; private set; }
    public int maxHealth { get; private set; }
    
    Rigidbody2D _rb;
    Animator _animator;
    PlayerMovement _movement;
    WeaponFSM _weaponFSM;
    [HideInInspector] public bool isInvulnerable;
    [SerializeField] float _damageKnockBackMultiplier;
    [SerializeField] float _cameraShakeMultiplier;
    [SerializeField] float _damageCameraShakeTimer;

    void Start ()
    {
        currHealth = GameMaster.Instance.savedPlayerData.CurrentHealth;
        maxHealth = GameMaster.Instance.savedPlayerData.MaxHealth;
        
        _rb = GetComponent<Rigidbody2D> ();
        _animator = GetComponent<Animator>();
        _movement = GetComponent<PlayerMovement>();
        _weaponFSM = GetComponentInChildren<WeaponFSM>();

        // Change Health UI
        HealthUI.Instance.UpdateHeartsUI();
    }

    public void TakeDamage(int damage, Vector2 enemyPos)
    {
        if (isInvulnerable) return;

        float frozenTime = 0.35f;

        currHealth -= damage;
        if (currHealth <= 0) {
            Die ();
            return;
        }

        // Disable Player Control
        Utility.EnablePlayerControl(false, frozenTime);

        // Freeze frame effect
        StartCoroutine(Utility.ChangeVariableAfterDelay<float>(e => Time.timeScale = e, 0.02f, 0.05f, Time.timeScale));

        // Shake Camera
        StartCoroutine(CameraShake.Instance.ShakeCameraAfterDelay(.02f, _cameraShakeMultiplier, _damageCameraShakeTimer));

        // Change Health UI
        HealthUI.Instance.UpdateHeartsUI();

        // Apply knockback force to player in opposite direction
        Vector2 dir = new Vector2 (_rb.position.x > enemyPos.x ? 1f : -1f, 1f);
        _movement.ApplyKnockback(dir, _damageKnockBackMultiplier, frozenTime);

        // Invulnerability Frames
        FindObjectOfType<InvulnerabilityFrames>().Flash();
    }

    public void PickUpHealth () {
        if (currHealth < maxHealth) {
            currHealth += 4;
        }

        // Change Health UI
        HealthUI.Instance.UpdateHeartsUI();
    }

    public void FullHeal () {
        currHealth = maxHealth;

        // Change Health UI
        HealthUI.Instance.UpdateHeartsUI();
    }

    void Die () {
        Destroy (gameObject);
        Debug.Log ("You died");

        GameMaster.Instance.RequestSceneChangeToMainMenu();

        // TODO: probably a game over screen
    }
}