using System;
using System.Collections;
using ChittaExorcist.GameCore;
using ChittaExorcist.PlayerSettings.InputHandler;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ChittaExorcist.ScreenSettings
{
    public class TransitionScreen : MonoBehaviour
    {
        // public static event Action SceneLoadingSucceeded;

        [SerializeField] private float fadeDuration = 1.0f;
        
        public static event Action ShowLoadingScreen;
        
        private WaitUntil _waitUntilSceneHasLoaded;

        private Image _transitionImage;

        #region w/ Coroutine

        private IEnumerator ActivateLoadedSceneCoroutine()
        {
            yield return _waitUntilSceneHasLoaded;
            SceneLoader.ActivateLoadedScene();
        }
        
        private IEnumerator ActivateLoadedScenesCoroutine()
        {
            yield return _waitUntilSceneHasLoaded;
            SceneLoader.ActivateLoadedScenes();
        }

        #endregion

        #region w/ Scene Load Fade

        private void OnSceneLoadFadeOut()
        {
            if (_transitionImage == null)
            {
                return;
            }
            
            _screenFadeTween?.Kill();

            _screenFadeTween = DOTween
                .To(() => _transitionImage.color, (value) => _transitionImage.color = value, new Color(0, 0, 0, 1),
                    fadeDuration)
                .SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
                {
                    if (SceneLoader.ShowLoadingScreen)
                    {
                        // Trigger show loading screen event
                        ShowLoadingScreen?.Invoke();
                    }
                    else
                    {
                        StartCoroutine(ActivateLoadedSceneCoroutine());
                    }
                });
        }

        #endregion
        
        #region w/ Fade Screen

        private Tween _screenFadeTween;
        
        private void OnFadeOut()
        {
            if (_transitionImage == null)
            {
                // DOTween 在 singleton 行為時銷毀會報錯誤
                // Debug.Log("Transition can not get image");
                return;
            }
            
            _screenFadeTween?.Kill();

            _screenFadeTween = DOTween
                .To(() => _transitionImage.color, (value) => _transitionImage.color = value, new Color(0, 0, 0, 1),
                    fadeDuration)
                .SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
                {
                    if (SceneLoader.ShowLoadingScreen)
                    {
                        // Trigger show loading screen event
                        ShowLoadingScreen?.Invoke();
                    }
                    else
                    {
                        // Debug.Log("Start");
                        // SceneLoadingSucceeded?.Invoke();
                        StartCoroutine(ActivateLoadedSceneCoroutine());
                    }
                });

        }
        
        private void OnFadesOut(bool boolValue)
        {
            if (_transitionImage == null)
            {
                // DOTween 在 singleton 行為時銷毀會報錯誤
                // Debug.Log("Transition can not get image");
                return;
            }
            
            _screenFadeTween?.Kill();

            _screenFadeTween = DOTween
                .To(() => _transitionImage.color, (value) => _transitionImage.color = value, new Color(0, 0, 0, 1),
                    fadeDuration)
                .SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
                {
                    if (SceneLoader.ShowLoadingScreen)
                    {
                        // Trigger show loading screen event
                        ShowLoadingScreen?.Invoke();
                    }
                    else if (boolValue)
                    {
                        // SceneLoadingSucceeded?.Invoke();
                        StartCoroutine(ActivateLoadedScenesCoroutine());
                    }
                });

        }

        private void OnFadeIn()
        {
            if (_transitionImage == null)
            {
                // DOTween 在 singleton 行為時銷毀會報錯誤
                // Debug.Log("Transition can not get image");
                return;
            }
            
            _screenFadeTween?.Kill();

            _screenFadeTween = DOTween
                .To(() => _transitionImage.color, (value) => _transitionImage.color = value, new Color(0, 0, 0, 0),
                    fadeDuration)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    InputManager.Instance.EnableCurrentInputs();
                });
        }

        #endregion        
        
        #region w/ Unity Callback Functions

        private void Awake()
        {
            _transitionImage = GetComponent<Image>();
            _waitUntilSceneHasLoaded = new WaitUntil(() => SceneLoader.IsSceneLoaded == true);

            SceneLoader.LoadingStarted += OnFadeOut;
            SceneLoader.LoadingsStarted += OnFadesOut;
            SceneLoader.LoadingCompleted += OnFadeIn;
        }

        #endregion
    }
}