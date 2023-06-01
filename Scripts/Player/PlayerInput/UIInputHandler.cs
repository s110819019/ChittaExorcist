using UnityEngine;
using UnityEngine.InputSystem;

namespace ChittaExorcist.PlayerSettings.InputHandler
{
    public class UIInputHandler : MonoBehaviour
    {
        // Submit
        public bool SubmitInput { get; private set; }
        // Unpause
        public bool UnpauseInput { get; private set; }

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
        
        // 互動確認輸入
        public void OnUnpauseInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                UnpauseInput = true;
            }
            else if (context.canceled)
            {
                UnpauseInput = false;
            }
        }
        public void UseUnpauseInput() => UnpauseInput = false;

        #endregion
        
        #region w/ Unity Callback Functions

        private void Start()
        {
            InputManager.Instance.OnUISubmit += OnSubmitInput;
            InputManager.Instance.OnUIUnpause += OnUnpauseInput;
        }

        private void OnDisable()
        {
            if (!InputManager.Instance)
            {
                // TODO: 取消運行時有錯誤
                return;
            }
            InputManager.Instance.OnUISubmit -= OnSubmitInput;
            InputManager.Instance.OnUIUnpause -= OnUnpauseInput;
        }

        #endregion
    }
}