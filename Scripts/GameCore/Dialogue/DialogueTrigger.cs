using System;
using ChittaExorcist.PlayerSettings.InputHandler;
using UnityEngine;

namespace ChittaExorcist.GameCore.DialogueSettings
{
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Ink JSON")] [SerializeField] private TextAsset inkJSON;
        [SerializeField] private TextAsset inkJSON_2;

        [Header("Visual Cue")] [SerializeField]
        private GameObject visualCue;

        public bool useSecondText;

        private bool _shouldUseSecondText;
        
        // [Header("Player State Check")]
        // [SerializeField] private PlayerStateChannel playerStateChannel;

        #region w/ Event

        public event Action OnDialogueEnter;
        public event Action<Transform> OnDialogueEnterCheckPlayerTransform;

        #endregion

        #region w/ Player State Check

        // private void OnCheckIsPlayerIdleState(bool value)
        // {
        //     _isPlayerIdleState = value;
        // }

        #endregion

        #region w/ Variables

        private bool _isPlayerInRange;

        // private bool _isPlayerIdleState;
        private bool _isPlaying;

        private Transform _playerLastTransform;

        #endregion

        #region w/ Components

        private PlayerInputHandler _inputHandler;
        private Animator _visualCueAnimator;

        #endregion

        #region w/ On Trigger

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerInRange = true;
                _playerLastTransform = other.transform;
                _inputHandler = other.GetComponent<PlayerInputHandler>();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerInRange = false;
                _inputHandler = null;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Debug.Log("Stay");
                _playerLastTransform = other.transform;
            }
        }

        #endregion

        #region w/ Unity Callback Functions

        private void Awake()
        {
            _isPlayerInRange = false;
            if (visualCue != null)
            {
                visualCue.TryGetComponent(out _visualCueAnimator);
                visualCue.SetActive(false);
            }
        }

        private void Update()
        {
            // 玩家在範圍內, 且目前非對話中
            if (_isPlayerInRange && !DialogueManager.Instance.IsPlayingDialogue)
            {
                if (visualCue != null)
                {
                    // 啟用對話提示
                    visualCue.SetActive(true);
                    if (!_isPlaying)
                    {
                        _visualCueAnimator.Play("Dialogue_Bubble_Start");
                        _isPlaying = true;
                    }
                }

                if (_inputHandler == null) return;

                // 按下互動開始對話
                if (_inputHandler.InteractInput)
                {


                    if (_shouldUseSecondText)
                    {
                        DialogueManager.Instance.EnterDialogueMode(inkJSON_2);
                    }
                    else
                    {
                        DialogueManager.Instance.EnterDialogueMode(inkJSON);
                        if (useSecondText && inkJSON_2 != null)
                        {
                            _shouldUseSecondText = true;
                        }                        
                    }

                    OnDialogueEnterCheckPlayerTransform?.Invoke(_playerLastTransform);
                    OnDialogueEnter?.Invoke();
                }
            }
            // 對話中 or 玩家不在範圍內
            else
            {
                if (visualCue != null)
                {
                    // 關閉對話提示
                    visualCue.SetActive(false);
                    _isPlaying = false;
                }
            }
        }

        #endregion
    }
}