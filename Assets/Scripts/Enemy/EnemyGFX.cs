using System.Collections;
using UnityEngine;

public class EnemyGFX : MonoBehaviour
{
    Transform _player;
    Animator _animator;
    SpriteRenderer _spriteRenderer;
    public float turningTime;
    bool _isTurning;
    [SerializeField] protected SpriteRenderer _exclaimationMark;
    [SerializeField] protected SpriteRenderer _questionMark;
    [SerializeField] protected ParticleSystem _deathParticleEffect;
    [SerializeField] protected ParticleSystem _damagedParticleEffect;

    void Start()
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
        StartCoroutine(FaceTowardsPlayerCoroutine(delay));
    }

    IEnumerator FaceTowardsPlayerCoroutine(float delay)
    {
        // Face Right
        if (_player.position.x >= _spriteRenderer.transform.position.x && _spriteRenderer.transform.localScale.x != 1.0f) {
            yield return new WaitForSeconds (delay);
            _spriteRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
            _questionMark.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        // Face Left
        else if (_player.position.x < _spriteRenderer.transform.position.x && _spriteRenderer.transform.localScale.x != -1.0f) {
            yield return new WaitForSeconds (delay);
            _spriteRenderer.transform.localScale = new Vector3(-1f, 1f, 1f);
            _questionMark.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    public void TurnAround(bool isInstant)
    {
        StartCoroutine(TurnAroundCoroutine(isInstant));
    }

    IEnumerator TurnAroundCoroutine(bool isInstant)
    {
        _isTurning = true;
        yield return new WaitForSeconds(isInstant ? 0.0f : turningTime);
        _spriteRenderer.transform.localScale = new Vector3(-_spriteRenderer.transform.localScale.x, 1f, 1f);
        _questionMark.transform.localScale = new Vector3(_spriteRenderer.transform.localScale.x, 1f, 1f);

        _isTurning = false;
    }

    public void FlashExclaimationMark()
    {
        StartCoroutine(FlashExclaimationMarkCoroutine());
    }
    IEnumerator FlashExclaimationMarkCoroutine()
    {
        _questionMark.enabled = false;
        _exclaimationMark.enabled = true;
        float flashTime = turningTime;

        // Flash at least 0.5 seconds
        if (flashTime < 0.5f) flashTime = 0.5f;
        yield return new WaitForSeconds (flashTime);
        _exclaimationMark.enabled = false;
    }

    public void FlashQuestionMark()
    {
        StartCoroutine(FlashQuestionMarkCoroutine());
    }
    public IEnumerator FlashQuestionMarkCoroutine ()
    {
        _exclaimationMark.GetComponent<SpriteRenderer>().enabled = false;
        _questionMark.enabled = true;
        float flashTime = turningTime;

        // Flash at least 0.5 seconds
        if (flashTime < 0.5f) flashTime = 0.5f;
        yield return new WaitForSeconds(flashTime);
        _questionMark.enabled = false;
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