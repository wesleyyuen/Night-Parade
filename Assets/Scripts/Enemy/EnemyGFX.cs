using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGFX : MonoBehaviour
{
    Transform _player;
    Animator _animator;
    public float turningTime;
    bool _isTurning;
    [SerializeField] protected GameObject _exclaimationMark;
    [SerializeField] protected GameObject _questionMark;
    [SerializeField] protected ParticleSystem _deathParticleEffect;
    [SerializeField] protected ParticleSystem _damagedParticleEffect;

    void Start ()
    {
        _player = GameObject.FindGameObjectWithTag ("Player").transform;
        _animator = GetComponent<Animator>();
    }

    public void SetAnimatorBoolean(string param, bool boolean)
    {
        _animator.SetBool(param, boolean);
    }

    public void SetAnimatorSpeed(float speed)
    {
        _animator.speed = speed;
    }

    public void FaceTowardsPlayer(float delay)
    {
        StartCoroutine(FaceTowardsPlayerCoroutine(delay));
    }

    public IEnumerator FaceTowardsPlayerCoroutine (float delay)
    {
        if (_player.position.x >= transform.position.x && transform.localScale.x != 1.0f) {
            yield return new WaitForSeconds (delay);
            transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);

            // Flip question mark back
            Utility.UnflipGameObjectRecursively(gameObject, true, true);

        } else if (_player.position.x < transform.position.x && transform.localScale.x != -1.0f) {
            yield return new WaitForSeconds (delay);
            transform.localScale = new Vector3 (-1.0f, 1.0f, 1.0f);
            
            // Flip question mark back
            Utility.UnflipGameObjectRecursively(gameObject, false, true);
        }
    }

    public void TurnAround(bool isInstant)
    {
        StartCoroutine(TurnAroundCoroutine(isInstant));
    }

    IEnumerator TurnAroundCoroutine(bool isInstant)
    {
        _isTurning = true;
        yield return new WaitForSeconds (isInstant ? 0.0f : turningTime);
        transform.localScale = new Vector3 (-transform.localScale.x, 1f, 1f);

        // Flip question mark back
        Utility.UnflipGameObjectRecursively(gameObject, false, false);
        _isTurning = false;
    }

    public void FlashExclaimationMark()
    {
        StartCoroutine(FlashExclaimationMarkCoroutine());
    }
    IEnumerator FlashExclaimationMarkCoroutine()
    {
        SpriteRenderer sr = _exclaimationMark.GetComponent<SpriteRenderer>();

        _questionMark.GetComponent<SpriteRenderer>().enabled = false;
        sr.enabled = true;
        float flashTime = turningTime;

        // Flash at least 0.5 seconds
        if (flashTime < 0.5f) flashTime = 0.5f;
        yield return new WaitForSeconds (flashTime);
        sr.enabled = false;
    }

    public void FlashQuestionMark()
    {
        StartCoroutine(FlashQuestionMarkCoroutine());
    }
    public IEnumerator FlashQuestionMarkCoroutine ()
    {
        SpriteRenderer sr = _questionMark.GetComponent<SpriteRenderer>();

        _exclaimationMark.GetComponent<SpriteRenderer>().enabled = false;
        sr.enabled = true;
        float flashTime = turningTime;

        // Flash at least 0.5 seconds
        if (flashTime < 0.5f) flashTime = 0.5f;
        yield return new WaitForSeconds(flashTime);
        sr.enabled = false;
    }

    public void PlayDamagedEffect()
    {
        GetComponent<SpriteFlash>().PlayDamagedFlashEffect();
        _damagedParticleEffect.Simulate(0, true, true);
        _damagedParticleEffect.Play();
    }

    public void PlayDeathEffect(float dieTime)
    {
        // GetComponent<SpriteFlash>().PlayDeathFlashEffect(dieTime);
        _animator.enabled = false;
        StartCoroutine(DeathEffectCoroutine(dieTime));
        _deathParticleEffect.Play();
    }

    IEnumerator DeathEffectCoroutine(float dieTime)
    {
        float flashDuration = 0.25f;
        SpriteFlash spriteFlash = GetComponent<SpriteFlash>();

        while (dieTime >= flashDuration) {
            yield return new WaitForSeconds (flashDuration);
            spriteFlash.PlayDeathFlashEffect(flashDuration);
            dieTime -= flashDuration;
        }
    }

    public bool IsTurning()
    {
        return _isTurning;
    }
}