using System;
using ChittaExorcist.Common.Module;
using ChittaExorcist.GameCore.DialogueSettings;
using ChittaExorcist.PlayerSettings.InputHandler;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace ChittaExorcist.UISettings
{
    public class UIInput : MonoSingleton<UIInput>
    {
        // [SerializeField] PlayerInput playerInput;
    
        private InputSystemUIInputModule _uiInputModule;

        // private PlayerInputHandler _playerInputHandler;

        protected override void Awake()
        {
            base.Awake();
            // TryGetComponent(out _playerInputHandler);
            _uiInputModule = GetComponent<InputSystemUIInputModule>();
            _uiInputModule.enabled = false;
        }

        private void Update()
        {
            if (DialogueManager.Instance.IsPlayingDialogue)
            {
                if (_uiInputModule.enabled == false)
                {
                    _uiInputModule.enabled = true;
                }
            }
        }

        public void SelectUI(Selectable selectableUIObject)
        {
            _uiInputModule.enabled = true;
            selectableUIObject.Select();
            selectableUIObject.OnSelect(null);
        }

        public void DisableAllUIInputs()
        {
            // playerInput.DisableAllInputs();
            // InputManager.Instance.DisableAllInputs();
            EventSystem.current.SetSelectedGameObject(null);
            _uiInputModule.enabled = false;
        }

        public void EnableAllUIInputs()
        {
            _uiInputModule.enabled = true;
        }
    }
}