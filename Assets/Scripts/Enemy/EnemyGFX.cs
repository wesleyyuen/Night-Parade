using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class EnemyGFX : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Transform _player;
    private bool _isTurning;
    [SerializeField] protected SpriteRenderer _exclaimationMark;
    [SerializeField] protected SpriteRenderer _questionMark;
    [SerializeField] protected ParticleSystem _deathParticleEffect;
    [SerializeField] protected ParticleSystem _damagedParticleEffect;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        foreach (Transform obj in transform) {
            if (obj.tag == "Sprite") {
                _animator = obj.GetComponent<Animator>();
                _spriteRenderer = obj.GetComponent<SpriteRenderer>();
                break;
            }
        }
    }

    public void SetAnimatorBoolean(string param, bool boolean)
    {
        if (!_animator)
            _animator = GetComponentInChildren<Animator>();
            
        _animator.SetBool(param, boolean);

    }

    public void SetAnimatorSpeed(float speed)
    {
        _animator.speed = speed;
    }

    public Vector2 GetEnemyScale()
    {
        return (Vector2)_spriteRenderer.transform.localScale;
    }

    public void SetEnemyColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void FaceTowardsPlayer(float delay)
    {
        Timing.RunCoroutine(_FaceTowardsPlayerCoroutine(delay).CancelWith(gameObject));
    }

    private IEnumerator<float> _FaceTowardsPlayerCoroutine(float delay)
    {
        // Face Right
        if (_player.position.x >= _spriteRenderer.transform.position.x && _spriteRenderer.transform.localScale.x != 1.0f) {
            _isTurning = true;
            yield return Timing.WaitForSeconds(delay);
            _spriteRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
            _questionMark.transform.localScale = new Vector3(1f, 1f, 1f);
            _isTurning = false;
        }
        // Face Left
        else if (_player.position.x < _spriteRenderer.transform.position.x && _spriteRenderer.transform.localScale.x != -1.0f) {
            _isTurning = true;
            yield return Timing.WaitForSeconds(delay);
            _spriteRenderer.transform.localScale = new Vector3(-1f, 1f, 1f);
            _questionMark.transform.localScale = new Vector3(-1f, 1f, 1f);
            _isTurning = false;
        }
    }

    public void TurnAround()
    {
        _isTurning = true;
        _spriteRenderer.transform.localScale = new Vector3(-_spriteRenderer.transform.localScale.x, 1f, 1f);
        _questionMark.transform.localScale = new Vector3(_spriteRenderer.transform.localScale.x, 1f, 1f);

        _isTurning = false;
    }

    public void PlayDamagedEffect()
    {
        GetComponent<SpriteFlash>().PlayDamagedFlashEffect();
        _damagedParticleEffect.Simulate(0, true, true);
        _damagedParticleEffect.Play();
    }

    public void PlayDeathEffect(float dieTime)
    {
        _animator.enabled = false;
        Timing.RunCoroutine(_DeathEffectCoroutine(dieTime).CancelWith(gameObject));
        _deathParticleEffect.Play();
    }

    private IEnumerator<float> _DeathEffectCoroutine(float dieTime)
    {
        float flashDuration = 0.25f;
        SpriteFlash spriteFlash = GetComponent<SpriteFlash>();

        while (dieTime >= flashDuration) {
            yield return Timing.WaitForSeconds(flashDuration);
            spriteFlash.PlayDeathFlashEffect(flashDuration);
            dieTime -= flashDuration;
        }
    }

    public bool IsTurning()
    {
        return _isTurning;
    }
}