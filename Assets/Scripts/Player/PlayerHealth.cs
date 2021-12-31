using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int currHealth { get; private set; }
    public int maxHealth { get; private set; }
    public delegate void Delegate_PlayerHealthChange (int prevHealth, float duration = 0.75f);
    public event Delegate_PlayerHealthChange Event_HealthChange;
    [SerializeField] private float _damagedKnockBackForce;
    [SerializeField] private float _cameraShakeMultiplier;
    [SerializeField] private float _damageCameraShakeTimer;
    [HideInInspector] public bool isInvulnerable;
    private Rigidbody2D _rb;
    private PlayerAnimations _anim;
    private PlayerMovement _movement;
    private WeaponFSM _weaponFSM;
    private bool _isDead;

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
        if (!isInvulnerable && !_isDead && !_weaponFSM.hasBlocked) {
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

        if (!_isDead && currHealth <= 0) {
            Die();
            return;
        }

        // Notify Observers
        Event_HealthChange?.Invoke(prevHealth);

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

    public void PickUpHealth()
    {
        int prevHealth = currHealth;

        if (currHealth < maxHealth) {
            currHealth += 4;
            currHealth = Mathf.Min(currHealth, maxHealth);

            // Change Health UI
            Event_HealthChange?.Invoke(prevHealth);
        }
    }

    public void FullHeal()
    {
        int prevHealth = currHealth;

        if (currHealth < maxHealth) {
            currHealth = maxHealth;

            // Change Health UI
            Event_HealthChange?.Invoke(prevHealth);
        }
    }

    private void Die()
    {
        Debug.Log ("You died");
        _isDead = true;
        StartCoroutine(_PlayGameOverAnimation());
    }

    // TODO: Can't use MEC for some reason
    private IEnumerator _PlayGameOverAnimation()
    {
        const float DURATION = 2.5f;

        // Turn off UIs
        GameMaster.Instance.SetUI(false);

        // TODO: player death animation instead of freezing
        _anim.FreezePlayerAnimation(DURATION);

        // Change Player Color
        GetComponent<SpriteFlash>().SetSpriteColor(Color.white);
        SpriteRenderer player = GetComponentInChildren<SpriteRenderer>();
        player.sortingLayerName = "UI";
        player.sortingOrder = 200;

        // TODO: Find better way to scale
        SpriteRenderer bg = GameObject.FindGameObjectWithTag("GameOver_BG").GetComponent<SpriteRenderer>();
        bg.enabled = true;

        yield return new WaitForSeconds(DURATION);

        // bg.DOFade(0f, 2f).OnComplete(() => bg.enabled = false);
        bg.enabled = false;
        GameMaster.Instance.RequestSceneChangeToMainMenu();
    }
}