using UnityEngine;

public class PickUp : MonoBehaviour
{
    const float ACTIVE_DURATION = 15f;
    const float BLINK_DURATION = 4f;
    // const float FAST_BLINK_DURATION = 1.5f;
    SpriteRenderer _renderer;
    Color _startingColor, _endColor;
    float _timer = 0f;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _startingColor = _renderer.color;
        _endColor = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 0f);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= ACTIVE_DURATION) {
            OnDestroy();
        // } else if (_timer >= (ACTIVE_DURATION - FAST_BLINK_DURATION)) {
        //     _renderer.color = Color.Lerp(_startingColor, _endColor, Mathf.PingPong(Time.time * 10f, 1f));
        } else if (_timer >= (ACTIVE_DURATION - BLINK_DURATION)) {
            _renderer.color = Color.Lerp(_startingColor, _endColor, Mathf.PingPong(Time.time * 6f, 1f));
        }
    }

    protected virtual void OnDestroy()
    {
        Destroy(gameObject);
    }
}