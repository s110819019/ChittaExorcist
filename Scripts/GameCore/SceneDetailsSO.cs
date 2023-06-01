using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ChittaExorcist.GameCore
{
    [CreateAssetMenu(fileName = "NewSceneData", menuName = "Custom Data/Scene/Scene Data")]
    public class SceneDetailsSO : ScriptableObject
    {
        public AssetReference sceneAsset;
        public String sceneName;
    }
}
