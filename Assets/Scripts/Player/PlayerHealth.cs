using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerHealth : MonoBehaviour
{
    public int currHealth { get; private set; }
    public int maxHealth { get; private set; }
    [SerializeField] private float _damagedKnockBackForce;
    [SerializeField] private float _cameraShakeMultiplier;
    [SerializeField] private float _damageCameraShakeTimer;
    [HideInInspector] public bool isInvulnerable;
    private Rigidbody2D _rb;
    private PlayerAnimations _anim;
    private PlayerMovement _movement;
    private PlayerAbilityController _abilities;
    private WeaponFSM _weaponFSM;
    private bool _isDead;

    private EventManager _eventManager;

    [Inject]
    public void Initialize(EventManager eventManager)
    {
        _eventManager = eventManager;
    }

    private void Start()
    {
        currHealth = SaveManager.Instance.savedPlayerData.CurrentHealth;
        maxHealth = SaveManager.Instance.savedPlayerData.MaxHealth;
        
        _rb = GetComponent<Rigidbody2D> ();
        _anim = GetComponent<PlayerAnimations>();
        _movement = GetComponent<PlayerMovement>();
        _abilities = GetComponent<PlayerAbilityController>();
        _weaponFSM = GetComponentInChildren<WeaponFSM>();

        // Change Health UI
        _eventManager.OnPlayerDamaged(currHealth, currHealth, maxHealth, 0f);
    }

    public void HandleDamage(int damage, Vector2 enemyPos)
    {
        if (!isInvulnerable && !_isDead && !_weaponFSM.hasBlocked) {
            // Stop attack
            if (_weaponFSM.IsCurrentState(WeaponStateType.Attack))
                _weaponFSM.SetState(_weaponFSM.GetStateByType(WeaponStateType.Idle));

            TakeDamage(damage, enemyPos);
        }
    }

    public void TakeDamage(int damage, Vector2 enemyPos)
    {
        if (isInvulnerable || damage <= 0) return;

        float frozenTime = 0.4f;
        int prevHealth = currHealth;

        currHealth -= damage;

        if (!_isDead && currHealth <= 0) {
            Die();
            return;
        }

        // Notify Observers
        _eventManager.OnPlayerDamaged(prevHealth, currHealth, maxHealth);

        // Disable Player Control
        Utility.EnablePlayerControl(false, frozenTime);
        // _abilities.EnableAbility(PlayerAbilityController.Ability.Dash, false, frozenTime);

        _anim.SetAttackAnimation(0);
        _anim.SetBlockAnimation(false);
        _anim.SetRunAnimation(0f);

        // Slow Motion effect
        TimeManager.Instance.SlowTimeForSeconds(0.02f, 0.35f);

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
            _eventManager.OnPlayerHealed(prevHealth, currHealth, maxHealth);
        }
    }

    public void FullHeal()
    {
        int prevHealth = currHealth;

        if (currHealth < maxHealth) {
            currHealth = maxHealth;

            // Change Health UI
            _eventManager.OnPlayerHealed(prevHealth, currHealth, maxHealth);
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

        _rb.isKinematic = true;
        _rb.velocity = Vector2.zero;
        // TODO: player death animation instead of freezing
        Utility.EnablePlayerControl(false);
        // _anim.FreezePlayerAnimation(true, DURATION);

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