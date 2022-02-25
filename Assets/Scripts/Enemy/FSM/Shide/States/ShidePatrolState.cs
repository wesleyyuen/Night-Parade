using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public sealed class ShidePatrolState : IEnemyState
{
    private ShideFSM _fsm;
    private const float FLOAT_DISTANCE = 1.5f;
    private const float FLOAT_DURATION = 1f;
    private Tweener _tweener;
    private Vector2 _originalPos;
    private float _lerpTime;
    private float _timer;
    private bool _stopUpdating;

    public ShidePatrolState(ShideFSM fsm)
    {
        _fsm = fsm;
    }

    public void EnterState()
    {
        _timer = 0f;
        _stopUpdating = false;

        _originalPos = _fsm.rb.position;
        DOTween.To(() => _lerpTime, x => _lerpTime = x, 1f, FLOAT_DURATION / 2).SetEase(Ease.InQuad);

        _fsm.GFX.SetAnimatorBoolean("IsPatrolling", true);
    }

    public void Update()
    {
        if (_stopUpdating) return;
        
        if (_timer >= FLOAT_DURATION / 2) {
            _timer = -1f;
            // Movement loop from bottom to top and back
            _tweener = _fsm.rb.DOMoveY(_originalPos.y + FLOAT_DISTANCE / 2, FLOAT_DURATION)
                        .SetEase(Ease.InOutQuad)
                        .SetLoops(-1, LoopType.Yoyo);
        } else if (_timer >= 0f) {
            _timer += Time.deltaTime;
            // Manually handle the inital middle to bottom movement
            _fsm.rb.position = (Vector3) Vector2.Lerp(_originalPos, _originalPos - new Vector2(0f, FLOAT_DISTANCE / 2), _lerpTime);
        }

        if (_fsm.IsInAggroRange() || _fsm.IsInLineOfSight()) {
            _fsm.SetState(_fsm.states[ShideStateType.Aggro]);
        }
    }

    public void FixedUpdate()
    {
    }

    public void OnCollisionEnter2D(Collision2D collision) {}
    public void OnCollisionStay2D(Collision2D collision) {}
    public void OnCollisionExit2D(Collision2D collision) {}
    public void ExitState()
    {
        _tweener?.Kill();
    }
}
