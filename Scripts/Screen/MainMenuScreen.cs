using System;
using System.Collections;
using System.Collections.Generic;
using ChittaExorcist.GameCore;
using ChittaExorcist.GameCore.AudioSettings;
using ChittaExorcist.PlayerSettings.InputHandler;
using ChittaExorcist.UISettings;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChittaExorcist.ScreenSettings
{
    public class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] private List<SceneDetailsSO> scenesToLoad = new List<SceneDetailsSO>();
        [SerializeField] private List<SceneDetailsSO> scenesToUnload = new List<SceneDetailsSO>();

        [SerializeField] private AudioDataSO mainMenuAudio;

        [Header("== Main Buttons ==")]
        [SerializeField] private Button starButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button quitButton;

        [Header("== Options Buttons ==")]
        [SerializeField] private Button optionsBackButton;
        [Header("Sound")]
        [SerializeField] private Button soundButton;
        [SerializeField] private Slider soundMainSlider;
        [SerializeField] private Button soundBackButton;
        [Header("Graphics")]
        [SerializeField] private Button graphicsButton;
        [SerializeField] private Button graphicsBackButton;
        [Header("Controls")]
        [SerializeField] private Button controlsButton;
        [SerializeField] private Button controlsBackButton;
        [Header("Producers")]
        [SerializeField] private Button producersButton;
        [SerializeField] private Button producersBackButton;

        [Header("Game Background")]
        [SerializeField] private GameObject mainMenuBackground;
        [SerializeField] private GameObject mainMenuBlurBackground;
        
        [Header("== Main Panel ==")]
        [SerializeField] private Canvas mainPanel;
        
        [Header("== Option Panel ==")]
        [SerializeField] private Canvas optionPanel;
        [Header("Sound")]
        [SerializeField] private Canvas soundOptionPanel;
        [Header("Graphics")]
        [SerializeField] private Canvas graphicsPanel;
        [Header("Controls")]
        [SerializeField] private Canvas controlsPanel;
        [Header("Producers")]
        [SerializeField] private Canvas producerPanel;

        private bool _showLoadingScreen;
        private bool _loadSceneAdditively;
        private bool _activateOnLoad;
        
        private List<AssetReference> _sceneAssetsToLoad = new List<AssetReference>();
        private List<String> _sceneNamesToUnload = new List<String>();

        #region w/ Unity Functions

        private void Start()
        {
            // if (PlayerManager.Instance)
            // {
            //     // TODO: 銷毀玩家方式可能待修正
            //     Destroy(PlayerManager.Instance.gameObject);
            // }

            var playerGO = GameObject.Find("Player");
            if (playerGO != null)
            {
                Destroy(playerGO);
            }

            Time.timeScale = 1.0f;
            UIInput.Instance.SelectUI(starButton);
            AudioManager.Instance.PlayAudio(mainMenuAudio);

            foreach (var sceneDetailsSo in scenesToLoad)
            {
                _sceneAssetsToLoad.Add(sceneDetailsSo.sceneAsset);
            }

            foreach (var sceneDetailsSo in scenesToUnload)
            {
                _sceneNamesToUnload.Add(sceneDetailsSo.sceneName);
            }
        }

        // private void Update()
        // {
        //     CheckSelectedButton();
        // }
        
        private void OnEnable()
        {
            // Start
            ButtonPressedBehaviour.buttonFunctionTable.Add(starButton.gameObject.name, StartGame);
            // Options
            ButtonPressedBehaviour.buttonFunctionTable.Add(optionsButton.gameObject.name, OpenOptionPanel);
            ButtonPressedBehaviour.buttonFunctionTable.Add(optionsBackButton.gameObject.name, CloseOptionPanel);
            // Quit
            ButtonPressedBehaviour.buttonFunctionTable.Add(quitButton.gameObject.name, QuitGame);
            // Sound
            ButtonPressedBehaviour.buttonFunctionTable.Add(soundButton.gameObject.name, OpenSoundPanel);
            ButtonPressedBehaviour.buttonFunctionTable.Add(soundBackButton.gameObject.name, CloseSoundPanel);
            
            // Graphics
            ButtonPressedBehaviour.buttonFunctionTable.Add(graphicsButton.gameObject.name, OpenGraphicsPanel);
            ButtonPressedBehaviour.buttonFunctionTable.Add(graphicsBackButton.gameObject.name, CloseGraphicsPanel);
            
            // Controls
            ButtonPressedBehaviour.buttonFunctionTable.Add(controlsButton.gameObject.name, OpenControlsPanel);
            ButtonPressedBehaviour.buttonFunctionTable.Add(controlsBackButton.gameObject.name, CloseControlsPanel);
            
            // Producers
            ButtonPressedBehaviour.buttonFunctionTable.Add(producersButton.gameObject.name, OpenProducerPanel);
            ButtonPressedBehaviour.buttonFunctionTable.Add(producersBackButton.gameObject.name, CloseProducerPanel);
        }


        #endregion

        #region w/ Selected UI Game Object

        private IEnumerator SetUISelectedGameObject(GameObject targetGameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(targetGameObject);
        }

        #endregion


        #region w/ Main Panel Work

        private void StartGame()
        {
            // TODO: Disable player input
            // game settings initialization
            // load default game scene
            // AudioManager.Instance.StopAudio(mainMenuAudio);
            // SceneLoader.LoadAddressableScene(newGameScene, _showLoadingScreen, false, false);
            // InputManager.Instance.SwitchToGameplayMap();
            // SceneLoader.LoadAddressableScenesThenUnloadScenes(_sceneAssetsToLoad.ConvertAll(x => (object) x),
            //     _sceneNamesToUnload.ConvertAll(x => (object) x), false, false);
            // SceneLoader.UnloadAddressableScene("MainMenuScene");
            SceneLoader.LoadAddressableScene(_sceneAssetsToLoad[0], _showLoadingScreen);
        }

        private void OpenOptionPanel()
        {
            mainPanel.enabled = false;
            optionPanel.enabled = true;
            mainMenuBackground.SetActive(false);
            mainMenuBlurBackground.SetActive(true);
            UIInput.Instance.SelectUI(soundButton);
        }

        private void QuitGame()
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            // AudioManager.Instance.StopAudio(mainMenuAudio);
        #else
            Application.Quit();
        #endif
        }        

        #endregion

        #region w/ Option Panel Work

        private void CloseOptionPanel()
        {
            mainPanel.enabled = true;
            optionPanel.enabled = false;
            mainMenuBackground.SetActive(true);
            mainMenuBlurBackground.SetActive(false);
            UIInput.Instance.SelectUI(optionsButton);
        }

        // Sound Open
        private void OpenSoundPanel()
        {
            optionPanel.enabled = false;
            soundOptionPanel.enabled = true;
            UIInput.Instance.SelectUI(soundMainSlider);
        }

        // Sound Close
        private void CloseSoundPanel()
        {
            optionPanel.enabled = true;
            soundOptionPanel.enabled = false;
            UIInput.Instance.SelectUI(soundButton);
        }
        
        // Graphics Open
        private void OpenGraphicsPanel()
        {
            optionPanel.enabled = false;
            graphicsPanel.enabled = true;
            UIInput.Instance.SelectUI(graphicsBackButton);
        }
        
        // Graphics Close
        private void CloseGraphicsPanel()
        {
            optionPanel.enabled = true;
            graphicsPanel.enabled = false;
            UIInput.Instance.SelectUI(graphicsButton);
        }
        
        // Controls Open
        private void OpenControlsPanel()
        {
            optionPanel.enabled = false;
            controlsPanel.enabled = true;
            UIInput.Instance.SelectUI(controlsBackButton);
        }
        
        // Controls Close
        private void CloseControlsPanel()
        {
            optionPanel.enabled = true;
            controlsPanel.enabled = false;
            UIInput.Instance.SelectUI(controlsButton);
        }
        
        // Producer Open
        private void OpenProducerPanel()
        {
            optionPanel.enabled = false;
            producerPanel.enabled= true;
            UIInput.Instance.SelectUI(producersBackButton);
        }
        
        // Producer Close
        private void CloseProducerPanel()
        {
            optionPanel.enabled = true;
            producerPanel.enabled= false;
            UIInput.Instance.SelectUI(producersButton);
        }

        #endregion
        

    }
}