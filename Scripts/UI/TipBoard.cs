using System;
using ChittaExorcist.EventChannel;
using ChittaExorcist.GameCore.DialogueSettings;
using ChittaExorcist.PlayerSettings.InputHandler;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ChittaExorcist.UISettings
{
    public class TipBoard : MonoBehaviour
    {
        [SerializeField] private Canvas tipCanvas;
        [SerializeField] private CanvasGroup tipPanelGroup;
        [SerializeField] private Canvas keyboardCanvas;
        [SerializeField] private Canvas gamepadPSCanvas;
        [SerializeField] private Canvas gamepadXboxCanvas;
        // [SerializeField] private GameObject tipGroup;
        [SerializeField] private float fadeAnimationTime = 0.5f;
        [SerializeField] private float fadeOutAnimationTime = 0.25f;
        [SerializeField] private float bounceAnimationTime = 0.5f;

        [SerializeField] private float disappearTime = 2.0f;

        [SerializeField] private VoidEventChannel tutorialEvent;
        
        private float _startTime;
        private bool _isPlayerIn;
        
        // private RectTransform _tipPanelGroupRectTransform;

        // [SerializeField] private UnityEvent onCheckCanUseSwitch;
        // [SerializeField] private UnityEvent onCheckCanUseSpecialSwitch;

        private void SetTipActive(bool value)
        {
            tipPanelGroup.DOKill();
            if (value) // 啟用淡入
            {
                tipCanvas.enabled = true;
                tipPanelGroup.transform.localScale = Vector3.zero;
                tipPanelGroup.alpha = 0;
                tipPanelGroup.DOFade(1, fadeAnimationTime)
                    .SetEase(Ease.Linear)
                    .SetUpdate(true);
                tipPanelGroup.transform.DOScale(1f, bounceAnimationTime).SetEase(Ease.OutBounce);
            }
            else // 關閉淡出
            {
                tipPanelGroup.DOFade(0, fadeOutAnimationTime)
                    .SetEase(Ease.Linear)
                    .SetUpdate(true)
                    .OnComplete(() => tipCanvas.enabled = false);
            }
        }

        #region w/ On Trigger

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _startTime = Time.time;
                _isPlayerIn = true;
                SetTipActive(true);
                // if (onCheckCanUseSwitch != null)
                // {
                //     onCheckCanUseSwitch?.Invoke();
                // }
                //
                // if (onCheckCanUseSpecialSwitch != null)
                // {
                //     onCheckCanUseSpecialSwitch?.Invoke();
                // }
                UIInput.Instance.EnableAllUIInputs();
                if (tutorialEvent != null)
                {
                    tutorialEvent.Broadcast();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerIn = false;
                SetTipActive(false);
                // if (!DialogueManager.Instance.IsPlayingDialogue)
                // {
                //     UIInput.Instance.DisableAllUIInputs();
                // }
            }
        }

        private void CheckTipTime()
        {
            if (!_isPlayerIn)
            {
                return;
            }

            if (Time.time >= _startTime + disappearTime)
            {
                SetTipActive(false);
                // if (!DialogueManager.Instance.IsPlayingDialogue)
                // {
                //     UIInput.Instance.DisableAllUIInputs();
                // }
            }
        }

        #endregion

        private void SwitchToKeyboard()
        {
            gamepadPSCanvas.enabled = false;
            gamepadXboxCanvas.enabled = false;
            keyboardCanvas.enabled = true;
        }
        
        private void SwitchToGamepadPS()
        {
            keyboardCanvas.enabled = false;
            gamepadXboxCanvas.enabled = false;
            gamepadPSCanvas.enabled = true;
        }
        
        private void SwitchToGamepadXbox()
        {
            keyboardCanvas.enabled = false;
            gamepadPSCanvas.enabled = false;
            gamepadXboxCanvas.enabled = true;
        }
        
        #region w/ Unity Callback Functions

        private void Awake()
        {
            // _tipPanelGroup = tipCanvas.GetComponent<CanvasGroup>();
            // _tipPanelGroupRectTransform = _tipPanelGroup.GetComponent<RectTransform>();
        }
        private void Start()
        {
            tipCanvas.enabled = false;
            
            // InputDeviceDetector.OnSwitchToMouse.AddListener(SwitchToMouse);
            InputDeviceDetector.OnSwitchToKeyboard.AddListener(SwitchToKeyboard);
            // InputDeviceDetector.OnSwitchToGamepad.AddListener(SwitchToGamepadPS);
            InputDeviceDetector.OnSwitchToGamepadPS.AddListener(SwitchToGamepadPS);
            InputDeviceDetector.OnSwitchToGamepadXbox.AddListener(SwitchToGamepadXbox);
        }

        private void Update()
        {
            CheckTipTime();
        }

        #endregion
    }
}