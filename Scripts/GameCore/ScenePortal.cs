using System;
using System.Collections.Generic;
using ChittaExorcist.PlayerSettings.InputHandler;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ChittaExorcist.GameCore
{
    public class ScenePortal : MonoBehaviour
    {
        // [SerializeField] private List<AssetReference> nextLoadScenes;
        [SerializeField] private List<SceneDetailsSO> scenesToLoad;
        [SerializeField] private List<SceneDetailsSO> scenesToUnload;
        [SerializeField] private Transform playerStartPoint;

        public ScenePortalLoadType loadSceneType = ScenePortalLoadType.LoadMultipleThenUnload;
        
        public bool showLoadingScreen;
        public bool loadSceneAdditively = true;
        public bool activateOnLoad;
        public bool shouldWaitForComplete = true;

        public bool isFacingDirectionRight = true;
        
        private bool _isPlayerIn;

        private List<AssetReference> _sceneAssetsToLoad = new List<AssetReference>();
        private List<String> _sceneNamesToUnload = new List<String>();

        private void OnSceneStartActivate()
        {
            // if (_isPlayerIn)
            // {
            //     // Debug.Log("Player Should Change Position");
            //     PlayerManager.Instance.SetTransform(playerStartPoint);
            //     PlayerManager.Instance.UnfreezePlayerY();
            //     PlayerManager.Instance.SetCurrentRespawnScene(scenesToLoad[0]);
            //     _isPlayerIn = false;
            // }
            // PlayerManager.Instance.FreezePlayerY();
            if (_isPlayerIn)
            {
                PlayerManager.Instance.SetTransform(playerStartPoint);
            }
        }

        private void GoToNextScene()
        {

            switch (loadSceneType)
            {
                case ScenePortalLoadType.LoadSingle:
                    SceneLoader.LoadAddressableScene(_sceneAssetsToLoad[0], showLoadingScreen, loadSceneAdditively, activateOnLoad);
                    break;
                case ScenePortalLoadType.LoadMultiple:
                    SceneLoader.LoadAddressableScenes(_sceneAssetsToLoad.ConvertAll(x => (object) x), showLoadingScreen, activateOnLoad);
                    break;
                case ScenePortalLoadType.LoadMultipleThenUnload:
                    SceneLoader.LoadAddressableScenesThenUnloadScenes(_sceneAssetsToLoad.ConvertAll(x => (object) x),
                        _sceneNamesToUnload.ConvertAll(x => (object) x), showLoadingScreen, activateOnLoad,
                        shouldWaitForComplete);
                    break;
                default:
                    break;
            }
            
            // SceneLoader.LoadAddressableScenesThenUnloadScenes(scenesToLoad.ConvertAll(x => (object) x),
            //     scenesToUnload.ConvertAll(x => (object) x), showLoadingScreen, activateOnLoad, shouldWaitForComplete);
            // SceneLoader.UnloadAddressableScene(sceneToUnload);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerIn = true;
                PlayerManager.Instance.SetCurrentRespawnScene(scenesToLoad[0]);
                PlayerManager.Instance.SetDefaultFacingDirection(isFacingDirectionRight);
                InputManager.Instance.DisableCurrentInputs();
                GoToNextScene();
            }
        }

        public enum ScenePortalLoadType
        {
            LoadSingle,
            LoadMultiple,
            LoadMultipleThenUnload
        }

        // private void OnTriggerExit2D(Collider2D other)
        // {
        //     if (other.CompareTag("Player"))
        //     {
        //         _isPlayerIn = false;
        //     }
        // }

        #region w/ Unity Functions

        private void Awake()
        {
            foreach (var sceneDetailsSo in scenesToLoad)
            {
                _sceneAssetsToLoad.Add(sceneDetailsSo.sceneAsset);
            }

            foreach (var sceneDetailsSo in scenesToUnload)
            {
                _sceneNamesToUnload.Add(sceneDetailsSo.sceneName);
            }
        }

        private void OnEnable()
        {
            SceneLoader.ActivatingLoadedScene += OnSceneStartActivate;
            // SceneLoader.LoadingSucceeded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            _isPlayerIn = false;
            SceneLoader.ActivatingLoadedScene -= OnSceneStartActivate;
            // SceneLoader.LoadingSucceeded -= OnSceneLoaded;
            // SceneLoader.LoadingCompletedBeforeUnload -= OnSceneLoaded;
        }

        #endregion
        

    }
}