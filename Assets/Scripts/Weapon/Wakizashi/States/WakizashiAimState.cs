// using UnityEngine;

// public sealed class WakizashiAimState : IWeaponState, IBindInput
// {
//     private InputManager _inputManager;
//     private readonly WakizashiFSM _fsm;
//     private PlayerAnimations _playerAnimation;
//     private bool _isThrowing, _stopUpdating;

//     public WakizashiAimState(WakizashiFSM fsm, InputManager inputManager)
//     {
//         _fsm = fsm;
//         _inputManager = inputManager;

//         _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
//     }

//     public void BindInput()
//     {
//         _inputManager.Event_GameplayInput_ThrowSlowTap += OnStartThrow;
//     }

//     public void UnbindInput()
//     {
//         _inputManager.Event_GameplayInput_ThrowSlowTap -= OnStartThrow;
//     }
 
//     public void EnterState()
//     {
//         _isThrowing = false;
//         _stopUpdating = false;

//         bool hasInput = _inputManager.HasDirectionalInput();
//         _fsm.throwDirection = hasInput ? _inputManager.GetDirectionalInputVector() : _playerAnimation.IsFacingRight() ? Vector2.right : Vector2.left;

//         Utility.EnablePlayerControl(false);
//     }

//     private void OnStartThrow()
//     {
//         if (_fsm.IsCurrentState(WakizashiStateType.Aim) && !_isThrowing) {
//             _isThrowing = true;
//         }
//     }

//     public void Update()
//     {
//         if (_stopUpdating) return;

//         if (_inputManager.HasDirectionalInput()) {
//             _fsm.throwDirection = _inputManager.GetDirectionalInputVector();
//         }

//         if (_isThrowing) {
//             _stopUpdating = true;
//             _fsm.SetState(_fsm.states[WakizashiStateType.Throw]);
//         }
//     }

//     public void FixedUpdate()
//     {
//     }

//     public void ExitState()
//     {
//         Utility.EnablePlayerControl(true);
//     }
// }
