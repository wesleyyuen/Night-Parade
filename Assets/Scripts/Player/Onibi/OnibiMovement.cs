using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnibiMovement : MonoBehaviour
{
    Transform _player;
    Rigidbody2D _rb;

    [Header ("Parameters")]
    [SerializeField] Vector2 offsetFromPlayer;
    Vector2 _refPosition;
    Vector2 _desiredPoint;

    private void Start()
    {
        _player = transform.parent.Find("Player").Find("Sprite");
        _rb = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(_player.position.x + (_player.localScale.x == 1 ? offsetFromPlayer.x : -offsetFromPlayer.x),
                                         _player.position.y + offsetFromPlayer.y,
                                         0);
        _desiredPoint = transform.position;

        // Overclock glow intensity
        Material mat = GetComponent<SpriteRenderer>().material;
        float factor = Mathf.Pow(2, 3);
        mat.color = new Color(mat.color.r * factor, mat.color.g * factor, mat.color.b * factor, mat.color.a);
    }

    private void FixedUpdate()
    {
        _desiredPoint = new Vector2(_player.position.x + (_player.localScale.x == 1 ? offsetFromPlayer.x : -offsetFromPlayer.x),
                                   _player.position.y + offsetFromPlayer.y + Mathf.Sin(Time.time * Random.value) * 0.5f);
        _rb.position = Vector2.SmoothDamp(_rb.position, _desiredPoint, ref _refPosition, 0.3f, 15, Time.deltaTime); 
    }
}
