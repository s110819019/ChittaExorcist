using System;
using ChittaExorcist.GameCore;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;

namespace ChittaExorcist.ScreenSettings
{
    public class InitScreen : MonoBehaviour
    {
        [SerializeField] private SceneDetailsSO menuScene;

        private void Awake()
        {
            DebugManager.instance.enableRuntimeUI = false;
        }

        private void Start()
        {
            SceneLoader.LoadAddressableScene(menuScene.sceneAsset, false);
        }
    }
}