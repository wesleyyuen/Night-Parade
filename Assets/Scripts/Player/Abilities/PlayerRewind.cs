using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerRewind : MonoBehaviour
{
    [Header ("References")]
    private InputManager _inputManager;
    private Rigidbody2D _rb;
    private PlayerHealth _health;
    private PlayerAbilityController _abilities;
    private PlayerAnimations _anim;
    private PlayerMovement _movement;
    private List<PointInTime> _pointsInTime;
    [Header ("Rewind Settings")]
    [SerializeField] private float _rewindDuration;
    private float _timer;
    private bool _isRewinding;

    [Inject]
    public void Initialize(InputManager inputManager)
    {
        _inputManager = inputManager;
    }

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _health = GetComponentInParent<PlayerHealth>();
        _abilities = GetComponentInParent<PlayerAbilityController>();
        _anim = transform.parent.gameObject.GetComponent<PlayerAnimations>();
        _movement = transform.parent.gameObject.GetComponent<PlayerMovement>();

        _pointsInTime = new List<PointInTime>();
        _timer = 0f;
        _isRewinding = false;
    }

    private void OnEnable()
    {
        _inputManager.Event_GameplayInput_Rewind += StartRewind;
    }

    private void OnDisable()
    {
        _inputManager.Event_GameplayInput_Rewind -= StartRewind;
    }

    private void Update()
    {
        if (_isRewinding && _timer > 0f) {
            Rewind();

            _timer -= Time.deltaTime;
            if (_timer <= 0f) {
                EndRewind();
            }
        }
        else Record();
    }

    private void StartRewind()
    {
        if (_isRewinding) return;
        
        _isRewinding = true;
        _health.isInvulnerable = true;
        _timer = _rewindDuration;
        _movement.EnablePlayerMovement(false);
        _abilities.EnableAbilityExcept(PlayerAbilityController.Ability.Rewind, false);
    }

    private void EndRewind()
    {
        _isRewinding = false;
        _health.isInvulnerable = false;
        _movement.EnablePlayerMovement(true);
        _abilities.EnableAbilityExcept(PlayerAbilityController.Ability.Rewind, true);
    }

    private void Rewind()
    {
        if (_pointsInTime.Count > 0) {
            PointInTime pointInTime = _pointsInTime[0];
            _rb.position = pointInTime.Position;
            // _rb.velocity = -pointInTime.Velocity;
            _anim.FaceRight(pointInTime.IsFacingRight);

            _pointsInTime.RemoveAt(0);
        }
    }

    private void Record()
    {
        if (_pointsInTime.Count > Mathf.Round(_rewindDuration / Time.deltaTime)) {
            _pointsInTime.RemoveAt(_pointsInTime.Count - 1);
        }

        _pointsInTime.Insert(0, new PointInTime(_rb.position, _rb.velocity, _anim.IsFacingRight()));
    }
}

public struct PointInTime
{
    public Vector3 Position;
    public Vector2 Velocity;
    public bool IsFacingRight;

    public PointInTime(Vector3 position, Vector2 velocity, bool isFacingRight)
    {
        Position = position;
        Velocity = velocity;
        IsFacingRight = isFacingRight;
    }
}