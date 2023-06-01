using System;
using System.Collections.Generic;
using ChittaExorcist.GameCore;
using ChittaExorcist.GameCore.AudioSettings;
using ChittaExorcist.PlayerSettings.InputHandler;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace ChittaExorcist.UISettings
{
    public class CutSceneTip : MonoBehaviour
    {
        [SerializeField] private AudioDataSO mainMenuAudio;
        [SerializeField] private AudioDataSO sceneAudio;
        [SerializeField] private List<SceneDetailsSO> scenesToLoad = new List<SceneDetailsSO>();
        [SerializeField] private List<SceneDetailsSO> scenesToUnload = new List<SceneDetailsSO>();
        [SerializeField] private Canvas keyboardCanvas;
        [SerializeField] private Canvas gamepadPSCanvas;
        [SerializeField] private Canvas gamepadXboxCanvas;

        private bool _showLoadingScreen;
        private bool _loadSceneAdditively;
        private bool _activateOnLoad;

        private bool _hasPressed;
        
        private List<AssetReference> _sceneAssetsToLoad = new List<AssetReference>();
        private List<String> _sceneNamesToUnload = new List<String>();
        
        [SerializeField] Button continueButton;
        
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

        public void StartGame()
        {
            if (_hasPressed)
            {
                return;
            }

            _hasPressed = true;
            // Debug.Log("Start");
            InputManager.Instance.SwitchToGameplayMap();
            AudioManager.Instance.PlayAudio(sceneAudio);
            SceneLoader.LoadAddressableScene(_sceneAssetsToLoad[0], _showLoadingScreen);
        }
        
        #region w/ Unity Callback Functions

        // private void OnEnable()
        // {
        //     ButtonPressedBehaviour.buttonFunctionTable.Add(continueButton.gameObject.name, StartGame);
        // }
        
        private void Start()
        {
            UIInput.Instance.SelectUI(continueButton);
            AudioManager.Instance.StopAudio(mainMenuAudio);
            // AudioManager.Instance.PlayAudio(sceneAudio);

            foreach (var sceneDetailsSo in scenesToLoad)
            {
                _sceneAssetsToLoad.Add(sceneDetailsSo.sceneAsset);
            }

            foreach (var sceneDetailsSo in scenesToUnload)
            {
                _sceneNamesToUnload.Add(sceneDetailsSo.sceneName);
            }
            
            // InputDeviceDetector.OnSwitchToMouse.AddListener(SwitchToMouse);
            InputDeviceDetector.OnSwitchToKeyboard.AddListener(SwitchToKeyboard);
            // InputDeviceDetector.OnSwitchToGamepad.AddListener(SwitchToGamepadPS);
            InputDeviceDetector.OnSwitchToGamepadPS.AddListener(SwitchToGamepadPS);
            InputDeviceDetector.OnSwitchToGamepadXbox.AddListener(SwitchToGamepadXbox);
        }

        #endregion
    }
}