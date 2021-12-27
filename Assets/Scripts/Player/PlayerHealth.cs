using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currHealth { get; private set; }
    public int maxHealth { get; private set; }
    public delegate void Delegate_PlayerHealthChange (int prevHealth, float duration = 0.75f);
    public event Delegate_PlayerHealthChange Event_HealthChange;
    
    private Rigidbody2D _rb;
    private PlayerAnimations _anim;
    private PlayerMovement _movement;
    private WeaponFSM _weaponFSM;
    [HideInInspector] public bool isInvulnerable;
    [SerializeField] private float _damagedKnockBackForce;
    [SerializeField] private float _cameraShakeMultiplier;
    [SerializeField] private float _damageCameraShakeTimer;

    private void Start()
    {
        currHealth = SaveManager.Instance.savedPlayerData.CurrentHealth;
        maxHealth = SaveManager.Instance.savedPlayerData.MaxHealth;
        
        _rb = GetComponent<Rigidbody2D> ();
        _anim = GetComponent<PlayerAnimations>();
        _movement = GetComponent<PlayerMovement>();
        _weaponFSM = GetComponentInChildren<WeaponFSM>();

        // Change Health UI
        Event_HealthChange?.Invoke(currHealth, 0f);
    }

    public void HandleDamage(int damage, Vector2 enemyPos)
    {
        if (!isInvulnerable || !_weaponFSM.hasBlocked) {
            // Stop attack
            if (_weaponFSM.IsCurrentState(WeaponFSM.StateType.AttackState))
                _weaponFSM.SetState(_weaponFSM.GetStateByType(WeaponFSM.StateType.IdleState));

            TakeDamage(damage, enemyPos);
        }
    }

    public void TakeDamage(int damage, Vector2 enemyPos)
    {
        if (isInvulnerable) return;

        float frozenTime = 0.5f;
        int prevHealth = currHealth;

        currHealth -= damage;

        // Notify Observers
        Event_HealthChange?.Invoke(prevHealth);

        if (currHealth <= 0) {
            Die();
            return;
        }

        // Disable Player Control
        Utility.EnablePlayerControl(false, frozenTime);
        _anim.SetAttackAnimation(0);
        _anim.SetBlockAnimation(false);
        _anim.SetRunAnimation(0f);

        // Slow Motion effect
        StartCoroutine(Utility._SlowTimeForSeconds(0.02f, 0.35f));

        // Shake Camera
        CameraShake.Instance.ShakeCamera(_cameraShakeMultiplier, _damageCameraShakeTimer);

        // Apply knockback force to player in opposite direction
        Vector2 dir = new Vector2 (_rb.position.x > enemyPos.x ? 1f : -1f, 1f);
        _movement.ApplyKnockback(dir, _damagedKnockBackForce, frozenTime);

        // Invulnerability Frames
        FindObjectOfType<InvulnerabilityFrames>().Flash();
    }

    public void PickUpHealth() {
        int prevHealth = currHealth;

        if (currHealth < maxHealth) {
            currHealth += 4;
            currHealth = Mathf.Min(currHealth, maxHealth);

            // Change Health UI
            Event_HealthChange?.Invoke(prevHealth);
        }
    }

    public void FullHeal() {
        int prevHealth = currHealth;

        if (currHealth < maxHealth) {
            currHealth = maxHealth;

            // Change Health UI
            Event_HealthChange?.Invoke(prevHealth);
        }
    }

    private void Die() {
        Destroy(gameObject);
        Debug.Log ("You died");

        GameMaster.Instance.RequestSceneChangeToMainMenu();

        // TODO: probably a game over screen
    }
}