using System;
using ChittaExorcist.PlayerSettings.InputHandler;
using ChittaExorcist.GameCore;
using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace ChittaExorcist.UISettings
{
    public class GameplayUIController : MonoBehaviour
    {
        [SerializeField] private AudioDataSO sceneAudio;
        [Header("Menu Scene")]
        [SerializeField] private SceneDetailsSO mainMenuScene;
        [Header("UI")]
        [SerializeField] private Canvas playerStatsUI;
        [SerializeField] private Canvas pauseUI;
        [Header("Buttons")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button optionButton;
        [SerializeField] private Button backToMenuButton;
        
        private UIInputHandler _uiInputHandler;
        private PlayerInputHandler _playerInputHandler;

        private bool _isPause;
        
        // 暫停
        private void Pause()
        {
            _isPause = true;
            
            UIInput.Instance.EnableAllUIInputs();
            UIInput.Instance.SelectUI(resumeButton);
            
            Time.timeScale = 0.0f;
            playerStatsUI.enabled = false;
            pauseUI.enabled = true;
            
            InputManager.Instance.SwitchToUIplayMap();
        }

        private void Unpause()
        {
            _isPause = false;
            
            UIInput.Instance.DisableAllUIInputs();

            Time.timeScale = 1.0f;
            playerStatsUI.enabled = true;
            pauseUI.enabled = false;
            
            InputManager.Instance.SwitchToGameplayMap();
        }
        
        // 確認輸入
        private void CheckInput()
        {
            if (_playerInputHandler.PauseInput && !_isPause)
            {
                _playerInputHandler.UsePauseInput();
                Pause();
            }

            if (_uiInputHandler.UnpauseInput && _isPause)
            {
                _uiInputHandler.UseUnpauseInput();
                Unpause();
            }
        }

        private void BackToMenu()
        {
            // Time.timeScale = 1.0f;
            UIInput.Instance.DisableAllUIInputs();

            AudioManager.Instance.StopAudio(sceneAudio);
            SceneLoader.LoadAddressableScene(mainMenuScene.sceneAsset, false);
        }
        
        #region w/ Unity Functions

        private void Awake()
        {
            TryGetComponent(out _uiInputHandler);
            TryGetComponent(out _playerInputHandler);
        }

        private void Update()
        {
            CheckInput();
        }

        private void OnEnable()
        {
            //TODO: Dictionary PlayerManager 銷毀時會重複添加的問題
            if (!ButtonPressedBehaviour.buttonFunctionTable.ContainsKey(resumeButton.gameObject.name))
            {
                ButtonPressedBehaviour.buttonFunctionTable.Add(resumeButton.gameObject.name, Unpause);
            }
            
            if (!ButtonPressedBehaviour.buttonFunctionTable.ContainsKey(backToMenuButton.gameObject.name))
            {
                ButtonPressedBehaviour.buttonFunctionTable.Add(backToMenuButton.gameObject.name, BackToMenu);
            }
        }

        // private void OnDisable()
        // {
        //     ButtonPressedBehaviour.buttonFunctionTable.Remove(resumeButton.gameObject.name);
        //     ButtonPressedBehaviour.buttonFunctionTable.Remove(backToMenuButton.gameObject.name);
        // }

        #endregion
    }
}