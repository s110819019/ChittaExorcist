using System;
using System.Collections;
using System.Collections.Generic;
using ChittaExorcist.Common.Module;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ChittaExorcist.PlayerSettings.InputHandler
{
    public class InputManager : MonoSingleton<InputManager>
    {
        // Gameplay
        private const string Gameplay = "Gameplay";
        private const string Gameplay_Move = "Move";
        private const string Gameplay_Jump = "Jump";
        private const string Gameplay_Dash = "Dash";
        private const string Gameplay_Attack = "Attack";
        private const string Gameplay_Switch = "Switch";
        private const string Gameplay_Block = "Block";
        private const string Gameplay_Charge = "Charge";
        private const string Gameplay_Interact = "Interact";
        private const string Gameplay_Submit = "Submit";
        private const string Gameplay_Pause = "Pause";
        
        // UI
        private const string UIplay = "UIplay";
        private const string UIplay_Submit = "Submit";
        private const string UIplay_Unpause = "Unpause";
        
        private PlayerInput _playerInput;

        #region w/ Events

        // Gameplay
        public event Action<InputAction.CallbackContext> OnMove;
        public event Action<InputAction.CallbackContext> OnJump;
        public event Action<InputAction.CallbackContext> OnDash;
        public event Action<InputAction.CallbackContext> OnAttack;
        public event Action<InputAction.CallbackContext> OnSwitch;
        public event Action<InputAction.CallbackContext> OnBlock;
        public event Action<InputAction.CallbackContext> OnCharge;
        public event Action<InputAction.CallbackContext> OnInteract;
        public event Action<InputAction.CallbackContext> OnSubmit;
        public event Action<InputAction.CallbackContext> OnPause;

        // UIplay
        public event Action<InputAction.CallbackContext> OnUISubmit;
        public event Action<InputAction.CallbackContext> OnUIUnpause;

        #endregion

        public void DisableCurrentInputs()
        {
            _playerInput.currentActionMap.Disable();
        }

        public void EnableCurrentInputs()
        {
            _playerInput.currentActionMap.Enable();
        }

        public void SwitchToUIplayMap()
        {
            _playerInput.SwitchCurrentActionMap(UIplay);
        }

        public void SwitchToGameplayMap()
        {
            _playerInput.SwitchCurrentActionMap(Gameplay);
        }

        private void SubscribeGameplayActionsEvents()
        {
            SubActionsEvent(Gameplay, Gameplay_Move, Move);
            SubActionsEvent(Gameplay, Gameplay_Jump, Jump);
            SubActionsEvent(Gameplay, Gameplay_Dash, Dash);
            SubActionsEvent(Gameplay, Gameplay_Attack, Attack);
            SubActionsEvent(Gameplay, Gameplay_Switch, Switch);
            SubActionsEvent(Gameplay, Gameplay_Block, Block);
            SubActionsEvent(Gameplay, Gameplay_Charge, Charge);
            SubActionsEvent(Gameplay, Gameplay_Interact, Interact);
            SubActionsEvent(Gameplay, Gameplay_Submit, Submit);
            SubActionsEvent(Gameplay, Gameplay_Pause, Pause);
        }
        
        private void UnsubscribeGameplayActionsEvents()
        {
            UnsubActionsEvent(Gameplay, Gameplay_Move, Move);
            UnsubActionsEvent(Gameplay, Gameplay_Jump, Jump);
            UnsubActionsEvent(Gameplay, Gameplay_Dash, Dash);
            UnsubActionsEvent(Gameplay, Gameplay_Attack, Attack);
            UnsubActionsEvent(Gameplay, Gameplay_Switch, Switch);
            UnsubActionsEvent(Gameplay, Gameplay_Block, Block);
            UnsubActionsEvent(Gameplay, Gameplay_Charge, Charge);
            UnsubActionsEvent(Gameplay, Gameplay_Interact, Interact);
            UnsubActionsEvent(Gameplay, Gameplay_Submit, Submit);
            UnsubActionsEvent(Gameplay, Gameplay_Pause, Pause);
        }

        private void SubscribeUIGameplayActionsEvents()
        {
            SubActionsEvent(UIplay, UIplay_Submit, UISubmit);
            SubActionsEvent(UIplay, UIplay_Unpause, UIUnpause);
        }
        
        private void UnsubscribeUIGameplayActionsEvents()
        {
            UnsubActionsEvent(UIplay, UIplay_Submit, UISubmit);
            UnsubActionsEvent(UIplay, UIplay_Unpause, UIUnpause);
        }

        private void SubActionsEvent(string mapName, string actionName, Action<InputAction.CallbackContext> action)
        {
            if (_playerInput == null)
            {
                // Debug.Log($"PlayerInput is null when try sub {mapName} map");
                return;
            }
            _playerInput.actions.FindActionMap(mapName)[actionName].started += action;
            _playerInput.actions.FindActionMap(mapName)[actionName].performed += action;
            _playerInput.actions.FindActionMap(mapName)[actionName].canceled += action;
        }
        
        private void UnsubActionsEvent(string mapName, string actionName, Action<InputAction.CallbackContext> action)
        {
            if (_playerInput == null)
            {
                // Debug.Log($"PlayerInput is null when try unsub {mapName} map");
                return;
            }
            _playerInput.actions.FindActionMap(mapName)[actionName].started -= action;
            _playerInput.actions.FindActionMap(mapName)[actionName].performed -= action;
            _playerInput.actions.FindActionMap(mapName)[actionName].canceled -= action;
        }
        
        #region w/ Unity Functions

        protected override void Awake()
        {
            base.Awake();
            TryGetComponent(out _playerInput);
            if (_playerInput == null)
            {
                Debug.LogWarning("No Player Input On InputManager");
            }
        }

        private void Start()
        {
            // Gameplay
            SubscribeGameplayActionsEvents();
            SubscribeUIGameplayActionsEvents();
        }

        private void OnDisable()
        {
            // Gameplay
            UnsubscribeGameplayActionsEvents();
            UnsubscribeUIGameplayActionsEvents();
        }        

        #endregion

        #region w/ Gameplay Input Actions

        // Move
        private void Move(InputAction.CallbackContext context)
        {
            OnMove?.Invoke(context);
        }
        // Jump
        private void Jump(InputAction.CallbackContext context)
        {
            OnJump?.Invoke(context);
        }
        // Dash
        private void Dash(InputAction.CallbackContext context)
        {
            OnDash?.Invoke(context);
        }
        // Attack
        private void Attack(InputAction.CallbackContext context)
        {
            OnAttack?.Invoke(context);
        }
        // Switch
        private void Switch(InputAction.CallbackContext context)
        {
            OnSwitch?.Invoke(context);
        }
        // Block
        private void Block(InputAction.CallbackContext context)
        {
            OnBlock?.Invoke(context);
        }
        // Charge
        private void Charge(InputAction.CallbackContext context)
        {
            OnCharge?.Invoke(context);
        }
        // Interact
        private void Interact(InputAction.CallbackContext context)
        {
            OnInteract?.Invoke(context);
        }
        // Submit
        private void Submit(InputAction.CallbackContext context)
        {
            OnSubmit?.Invoke(context);
        }
        // Pause
        private void Pause(InputAction.CallbackContext context)
        {
            OnPause?.Invoke(context);
        }

        #endregion

        #region w/ UIplay Input Actions

        // Submit
        private void UISubmit(InputAction.CallbackContext context)
        {
            OnUISubmit?.Invoke(context);
        }
        // Unpause
        private void UIUnpause(InputAction.CallbackContext context)
        {
            OnUIUnpause?.Invoke(context);
        }

        #endregion
        
    }
}
