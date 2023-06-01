using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ChittaExorcist.Common.Module;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace ChittaExorcist.GameCore
{
    public class SceneLoader : MonoSingleton<SceneLoader>
    {
        #region w/ Events

        public static event Action LoadingStarted;   // 加載開始          畫面淡出(變暗)
        public static event Action<float> IsLoading; // 加載中            加載進度條
        public static event Action LoadingSucceeded; // 加載成功(等待啟動)  提示以繼續操作
        public static event Action LoadingCompleted; // 加載結束          畫面淡入(變亮)


        public static event Action<bool> LoadingsStarted;   // 加載開始   畫面淡出(變暗)
        public static event Action LoadingCompletedBeforeUnload;
        public static event Action ActivatingLoadedScene;
        
        

        #endregion
        
        #region w/ Variables

        // 取得自身實例以在靜態函式中呼叫協程
        // private static SceneLoader _instance;

        private static SceneInstance _loadedSceneInstance;
        private static List<SceneInstance> _loadedSceneInstances = new List<SceneInstance>();
        private static List<Object> _waitToUnloadedSceneObjects = new List<object>();
        private static bool _shouldWaitUnloadedScenesForLoadingCompleted;

        private const string DebugRoomSceneKey = "DebugRoomScene";
        private const string MainMenuSceneKey = "MainMenuScene";
        
        public static bool ShowLoadingScreen { get; private set; }
        public static bool IsSceneLoaded { get; private set; }

        private static readonly List<SceneInstance> _hasLoadedScenes = new List<SceneInstance>();

        private static List<SceneInstance> _tempLoadedScenes = new List<SceneInstance>();

        #endregion

        #region w/ Load Multi Scenes

        private static IEnumerator LoadAddressableScenesCoroutine(List<Object> sceneKeys,
            List<Object> sceneKeysToUnload, bool showLoadingScreen, bool activateOnLoad, bool shouldWaitForComplete)
        {
            if (sceneKeys.Count == 0)
            {
                Debug.LogWarning("No scene keys provided to load.");
                yield break;
            }

            var loadSceneMode = LoadSceneMode.Additive;
            var asyncOperationHandles = new List<AsyncOperationHandle<SceneInstance>>();

            foreach (var sceneKey in sceneKeys)
            {
                var asyncOperationHandle = Addressables.LoadSceneAsync(sceneKey, loadSceneMode, activateOnLoad);
                asyncOperationHandles.Add(asyncOperationHandle);
            }

            // 加載開始
            LoadingsStarted?.Invoke(true);
            ShowLoadingScreen = showLoadingScreen;

            _shouldWaitUnloadedScenesForLoadingCompleted = shouldWaitForComplete;

            bool allScenesLoaded = false;
            while (!allScenesLoaded)
            {
                allScenesLoaded = true;
                float totalPercentComplete = 0f;
                foreach (var asyncOperationHandle in asyncOperationHandles)
                {
                    if (asyncOperationHandle.Status != AsyncOperationStatus.Succeeded)
                    {
                        allScenesLoaded = false;
                    }

                    totalPercentComplete += asyncOperationHandle.PercentComplete;
                }

                // 加載中
                IsLoading?.Invoke(totalPercentComplete / asyncOperationHandles.Count); // 0 到 1 的浮點數
                yield return null;
            }

            _tempLoadedScenes.Clear();
            
            foreach (var asyncOperationHandle in asyncOperationHandles)
            {
                // _hasLoadedScenes.Add(asyncOperationHandle.Result);
                _tempLoadedScenes.Add(asyncOperationHandle.Result);
            }

            if (activateOnLoad)
            {
                // 加載完成
                LoadingCompleted?.Invoke();
                // 紀錄需要 Unload 的 Scenes
                if (sceneKeysToUnload != null && sceneKeysToUnload.Count != 0)
                {
                    _waitToUnloadedSceneObjects.Clear();
                    foreach (var item in sceneKeysToUnload)
                    {
                        _waitToUnloadedSceneObjects.Add(item);
                    }
                }
            }
            else
            {
                // 加載成功
                LoadingSucceeded?.Invoke();
                IsSceneLoaded = true;
                foreach (var asyncOperationHandle in asyncOperationHandles)
                {
                    _loadedSceneInstances.Add(asyncOperationHandle.Result);
                }
                // 紀錄需要 Unload 的 Scenes
                if (sceneKeysToUnload != null && sceneKeysToUnload.Count != 0)
                {
                    _waitToUnloadedSceneObjects.Clear();
                    foreach (var item in sceneKeysToUnload)
                    {
                        _waitToUnloadedSceneObjects.Add(item);
                    }
                }
            }

        }

        public static void ActivateLoadedScenes()
        {
            // Succeeded 後 由 TransitionScreen 的 Fade out complete 呼叫
            int numLoadedScenes = _loadedSceneInstances.Count;
            int numActivatedScenes = 0;
            
            foreach (SceneInstance sceneInstance in _loadedSceneInstances)
            {
                sceneInstance.ActivateAsync().completed += (op) =>
                {
                    numActivatedScenes++;

                    if (numActivatedScenes == numLoadedScenes)
                    {
                        OnActivatesCompleted(null);
                    }
                };
            }
        }
        
        // 所有場景都已經啟動
        private static void OnActivatesCompleted(AsyncOperation obj)
        {
            IsSceneLoaded = false;
            _loadedSceneInstances.Clear();
            LoadingCompletedBeforeUnload?.Invoke();
            // 目前場景都載入完成了
            // 接續取消指定的載入場景
            if (_waitToUnloadedSceneObjects != null && _waitToUnloadedSceneObjects.Count != 0)
            {
                UnloadAddressableScenes(_waitToUnloadedSceneObjects);
            }
            // 若不須等到取消載入行為結束的話
            if (!_shouldWaitUnloadedScenesForLoadingCompleted)
            {
                LoadingCompleted?.Invoke();
                UpdateHasLoadedScenes();
                if (_waitToUnloadedSceneObjects != null)
                {
                    _waitToUnloadedSceneObjects.Clear();
                }
            }
            
            // Debug.Log(_hasLoadedScenes.Count);
        }

        #endregion
        
        #region w/ Load Scene Coroutine

        private static IEnumerator LoadAddressableSceneCoroutine(object sceneKey, bool showLoadingScreen, bool loadSceneAdditively, bool activateOnLoad)
        {
            var loadSceneMode = loadSceneAdditively ? LoadSceneMode.Additive : LoadSceneMode.Single;
            // Debug.Log("Hi");
            var asyncOperationHandle = Addressables.LoadSceneAsync(sceneKey, loadSceneMode, activateOnLoad);
            
            // 加載開始
            LoadingStarted?.Invoke();
            ShowLoadingScreen = showLoadingScreen;

            while (asyncOperationHandle.Status != AsyncOperationStatus.Succeeded)
            {
                // 加載中
                IsLoading?.Invoke(asyncOperationHandle.PercentComplete); // 0 到 1 的浮點數

                yield return null;
            }

            if (!loadSceneAdditively)
            {
                _hasLoadedScenes.Clear();
            }
            _hasLoadedScenes.Add(asyncOperationHandle.Result);

            if (activateOnLoad)
            {
                // 加載完成
                LoadingCompleted?.Invoke();
                
                // TODO: activate on load
                LoadingCompletedBeforeUnload?.Invoke();

                yield break;
            }
            
            // 加載成功
            LoadingSucceeded?.Invoke();
            IsSceneLoaded = true;
            _loadedSceneInstance = asyncOperationHandle.Result;
        }

        public static void ActivateLoadedScene()
        {
            // Succeeded 後 由 TransitionScreen 的 Fade out complete 呼叫
            ActivatingLoadedScene?.Invoke();
            _loadedSceneInstance.ActivateAsync().completed += OnActivateCompleted;
        }

        private static void OnActivateCompleted(AsyncOperation obj)
        {
            IsSceneLoaded = false;
            _loadedSceneInstance = default;
            LoadingCompleted?.Invoke();
            LoadingCompletedBeforeUnload?.Invoke();
            // Debug.Log(_hasLoadedScenes);
        }
        
        #endregion

        private static void UpdateHasLoadedScenes()
        {
            foreach (var item in _tempLoadedScenes)
            {
                _hasLoadedScenes.Add(item);
            }
            _tempLoadedScenes.Clear();
        }
        
        #region w/ Load Scene

        // 因為此為持久化單一實例也不須繼承, 所以設為靜態
        public static void LoadAddressableScene(
            object sceneKey,
            bool showLoadingScreen,
            bool loadSceneAdditively = false,
            bool activateOnLoad = false)
        {
            // var loadSceneMode = loadSceneAdditively ? LoadSceneMode.Additive : LoadSceneMode.Single;
            // Addressables.LoadSceneAsync(sceneKey, loadSceneMode, activateOnLoad);
            Instance.StartCoroutine(LoadAddressableSceneCoroutine(sceneKey, showLoadingScreen, loadSceneAdditively, activateOnLoad));
        }

        
        public static void LoadAddressableScenesThenUnloadScenes(
            List<Object> sceneKeys,
            List<Object> sceneKeysToUnload,
            bool showLoadingScreen,
            bool activateOnLoad = false,
            bool shouldWaitForComplete = true)
        {
            Instance.StartCoroutine(LoadAddressableScenesCoroutine(sceneKeys, sceneKeysToUnload, showLoadingScreen, activateOnLoad, shouldWaitForComplete));
        }
        
        public static void LoadAddressableScenes(
            List<Object> sceneKeys,
            bool showLoadingScreen,
            bool activateOnLoad = false
           )
        {
            Instance.StartCoroutine(LoadAddressableScenesCoroutine(sceneKeys, null, showLoadingScreen, activateOnLoad, false));
        }
        
        #endregion

        #region w/ Unload Scene

        public static void UnloadAddressableScene(object sceneKey)
        {
            if (_hasLoadedScenes == null || _hasLoadedScenes.Count == 0)
            {
                Debug.LogWarning("There is no loaded scenes in hasLoadedList !");
                LoadingCompleted?.Invoke();
                _waitToUnloadedSceneObjects.Clear();
                return;
            }

            SceneInstance targetSceneInstance = _hasLoadedScenes.Find(x => x.Scene.name == (string) sceneKey);

            if (targetSceneInstance.Equals(default(SceneInstance)) || !targetSceneInstance.Scene.IsValid())
            {
                Debug.LogWarning($"The target scene: {targetSceneInstance} is not loaded or not valid !");
                LoadingCompleted?.Invoke();
                _waitToUnloadedSceneObjects.Clear();
                return;
            }

            _hasLoadedScenes.Remove(targetSceneInstance);
            Addressables.UnloadSceneAsync(targetSceneInstance).Completed += (asyncHandle) =>
            {
                Debug.Log($"Unload Scene: {sceneKey}");
                if (_shouldWaitUnloadedScenesForLoadingCompleted)
                {
                    _shouldWaitUnloadedScenesForLoadingCompleted = false;
                    LoadingCompleted?.Invoke();
                    _waitToUnloadedSceneObjects.Clear();
                }
            }; 
        }

        private static void UnloadAddressableScenes(List<Object> sceneKeys, bool waitForFade = false)
        {
            List<Object> tempKeys = new List<object>();

            if (sceneKeys == null || sceneKeys.Count == 0)
            {
                tempKeys = null;
            }
            else
            {
                foreach (var item in sceneKeys)
                {
                    tempKeys.Add(item);
                }                
            }

            if (_hasLoadedScenes == null || _hasLoadedScenes.Count == 0)
            {
                LoadingCompleted?.Invoke();
                UpdateHasLoadedScenes();
                _waitToUnloadedSceneObjects.Clear();
                Debug.LogWarning("There is no loaded scenes to unload!");
                return;
            }

            if (tempKeys == null || tempKeys.Count == 0)
            {
                LoadingCompleted?.Invoke();
                UpdateHasLoadedScenes();
                _waitToUnloadedSceneObjects.Clear();
                Debug.LogWarning("No scene keys provided to Unload.");
                return;
            }
            
            int numUnloadedScenes = tempKeys.Count;
            int numCurrentScene = 0;
            
            foreach (var sceneKey in tempKeys)
            {
                SceneInstance targetSceneInstance = _hasLoadedScenes.Find(x => x.Scene.name == (string) sceneKey);

                if (targetSceneInstance.Equals(default(SceneInstance)) || !targetSceneInstance.Scene.IsValid())
                {
                    Debug.LogWarning($"The target scene {(string) sceneKey} is not loaded !");
                    LoadingCompleted?.Invoke();
                    UpdateHasLoadedScenes();
                    _waitToUnloadedSceneObjects.Clear();
                }
                else
                {
                    _hasLoadedScenes.Remove(targetSceneInstance);
                    Addressables.UnloadSceneAsync(targetSceneInstance).Completed += (asyncHandle) =>
                    {
                        numCurrentScene++;
                        
                        // Debug.Log($"Unload {sceneKey}");
                        
                        if (numCurrentScene == numUnloadedScenes)
                        {
                            if (_shouldWaitUnloadedScenesForLoadingCompleted)
                            {
                                _shouldWaitUnloadedScenesForLoadingCompleted = false;
                                LoadingCompleted?.Invoke();
                                UpdateHasLoadedScenes();
                                _waitToUnloadedSceneObjects.Clear();
                            }
                        }
                    };                     
                }

               
            }
            
 
        }

        #endregion

        #region w/ Unity Callback Functions

        // private void Awake()
        // {
        //     _instance = this;
        // }

        #endregion
    }
}