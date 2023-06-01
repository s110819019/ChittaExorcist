using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ChittaExorcist.PlayerSettings.InputHandler
{
    /// <summary>
    /// 玩家輸入處理
    /// </summary>
    public class PlayerInputHandler : MonoBehaviour
    {
        #region w/ Time Delay Check

        [SerializeField] private float inputHoldTime = 0.2f;

        #endregion

        #region w/ Components

        // private PlayerInput _playerInput;

        #endregion

        #region w/ Input Start and Stop Time

        private float _jumpInputStartTime;
        public float AttackInputStartTime { get; private set; }
        public float AttackInputStopTime { get; private set; }

        #endregion

        #region w/ Input Variables

        // Move
        private Vector2 _rawMovementInput;
        public int NormalizedXInput { get; private set; }
        public int NormalizedYInput { get; private set; }

        // Jump
        public bool JumpInput { get; private set; }
        public bool JumpInputStop { get; private set; }

        // Dash
        public bool DashInput { get; private set; }

        // Attack
        public bool AttackInput { get; private set; }
        
        // Switch
        // TODO: 等待修正
        public bool SwitchInput { get; private set; }
        
        
        public bool BlockInput { get; private set; }
        
        public bool ChargeInput { get; private set; }
        
        
        // Interact
        public bool InteractInput { get; private set; }
        // Submit
        public bool SubmitInput { get; private set; }
        
        // Pause
        public bool PauseInput { get; private set; }

        #endregion

        #region w/ Unity Callback Functions

        private void Start()
        {
            InputManager.Instance.OnMove += OnMovementInput;
            InputManager.Instance.OnJump += OnJumpInput;
            InputManager.Instance.OnDash += OnDashInput;
            InputManager.Instance.OnAttack += OnAttackInput;
            InputManager.Instance.OnSwitch += OnSwitchInput;
            InputManager.Instance.OnBlock += OnBlockInput;
            InputManager.Instance.OnCharge += OnChargeInput;
            InputManager.Instance.OnInteract += OnInteractInput;
            InputManager.Instance.OnSubmit += OnSubmitInput;
            InputManager.Instance.OnPause += OnPauseInput;
        }

        private void OnDisable()
        {
            if (!InputManager.Instance)
            {
                // TODO: 取消運行時有錯誤
                return;
            }
            InputManager.Instance.OnMove -= OnMovementInput;
            InputManager.Instance.OnJump -= OnJumpInput;
            InputManager.Instance.OnDash -= OnDashInput;
            InputManager.Instance.OnAttack -= OnAttackInput;
            InputManager.Instance.OnSwitch -= OnSwitchInput;
            InputManager.Instance.OnBlock -= OnBlockInput;
            InputManager.Instance.OnCharge -= OnChargeInput;
            InputManager.Instance.OnInteract -= OnInteractInput;
            InputManager.Instance.OnSubmit -= OnSubmitInput;
            InputManager.Instance.OnPause -= OnPauseInput;
        }

        private void Update()
        {
            CheckJumpInputHoldTime();
        }

        #endregion

        #region w/ Movement

        // 移動
        public void OnMovementInput(InputAction.CallbackContext context)
        {
            _rawMovementInput = context.ReadValue<Vector2>();
            // Debug.Log(_rawMovementInput);
            NormalizedXInput = Mathf.RoundToInt(_rawMovementInput.x);
            NormalizedYInput = Mathf.RoundToInt(_rawMovementInput.y);
        }

        #endregion

        #region w/ Jump

        // 跳躍
        public void OnJumpInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                JumpInput = true;
                JumpInputStop = false;
                _jumpInputStartTime = Time.time;
            }

            if (context.canceled)
            {
                JumpInputStop = true;
            }
        }

        public void UseJumpInput() => JumpInput = false;

        private void CheckJumpInputHoldTime()
        {
            if (Time.time >= _jumpInputStartTime + inputHoldTime)
            {
                JumpInput = false;
            }
        }

        #endregion

        #region w/ Dash

        // 衝刺
        public void OnDashInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                DashInput = true;
            }
            else if (context.canceled)
            {
                DashInput = false;
            }
        }

        public void UseDashInput() => DashInput = false;

        #endregion

        #region w/ Attack

        // 普通攻擊
        public void OnAttackInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                AttackInput = true;
                AttackInputStartTime = Time.time;
            }

            if (context.canceled)
            {
                AttackInput = false;
                AttackInputStopTime = Time.time;
            }
        }

        public void UseAttackInput() => AttackInput = false;

        #endregion
        
        #region w/ Switch

        // 跳躍
        public void OnSwitchInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                SwitchInput = true;
            }

            if (context.canceled)
            {
                SwitchInput = false;
            }
        }

        public void UseSwitchInput() => SwitchInput = false;

        #endregion

        #region w/ Block

        // 防禦
        public void OnBlockInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                BlockInput = true;
            }
            else if (context.canceled)
            {
                BlockInput = false;
            }
        }

        public void UseBlockInput() => BlockInput = false;

        #endregion
        
        #region w/ Charge

        // 防禦
        public void OnChargeInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                ChargeInput = true;
            }
            else if (context.canceled)
            {
                ChargeInput = false;
            }
        }

        public void UseChargeInput() => ChargeInput = false;

        #endregion
        
        #region w/ Interact
        // 互動輸入
        public void OnInteractInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                InteractInput = true;
            }
            else if (context.canceled)
            {
                InteractInput = false;
            }
        }

        public void UseInteractInput() => InteractInput = false;

        #endregion
    
        #region w/ Submit
        // 互動確認輸入
        public void OnSubmitInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                SubmitInput = true;
            }
            else if (context.canceled)
            {
                SubmitInput = false;
            }
        }

        public void UseSubmitInput() => SubmitInput = false;

        #endregion
        
        #region w/ Pause
        // 暫停輸入
        public void OnPauseInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                PauseInput = true;
            }
            else if (context.canceled)
            {
                PauseInput = false;
            }
        }

        public void UsePauseInput() => PauseInput = false;

        #endregion
    }
}