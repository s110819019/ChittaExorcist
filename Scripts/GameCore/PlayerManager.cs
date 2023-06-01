using System;
using System.Collections.Generic;
using ChittaExorcist.Common.Module;
using ChittaExorcist.Common.Variables;
using ChittaExorcist.EventChannel;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

namespace ChittaExorcist.GameCore
{
    public class PlayerManager : MonoSingleton<PlayerManager>
    {
        [SerializeField] private GameObject playerGO;
        [SerializeField] private SceneDetailsSO initialSceneData;

        [Header("Listen Events")]
        [SerializeField] private VoidEventChannel onPlayerDeath;

        [Header("Broadcast Events")]
        [SerializeField] private FloatEventChannel onPlayerHealthChange;
        [SerializeField] private FloatEventChannel onPlayerManaChange;
        [SerializeField] private VoidEventChannel onPlayerRespawn;
        [SerializeField] public VoidEventChannel onPlayerFlip;

        [Header("Health")]
        [SerializeField] private FloatReference playerHealth;
        [SerializeField] private FloatReference playerMaxHealth;
        
        [Header("Mana")]
        [SerializeField] private bool resetManaWhenRespawn;
        [SerializeField] private FloatReference playerMana;
        [SerializeField] private FloatReference playerMaxMana;

        private SceneDetailsSO _sceneDetails;
        private List<AssetReference> _sceneAssetsToLoad = new List<AssetReference>();
        private List<String> _sceneNamesToUnload = new List<String>();

        private Rigidbody2D _rigidbody;
        private bool _isPlayerDeath;
        private int _currentDefaultFacingDirection;

        public void SetTransform(Transform targetTransform)
        {
            transform.position = targetTransform.position;
            ResetChildrenTransform();
        }

        public void SetDefaultFacingDirection(bool value)
        {
            if (value)
            {
                _currentDefaultFacingDirection = 1;
            }
            else
            {
                _currentDefaultFacingDirection = -1;
            }
        }

        public void SetCurrentRespawnScene(SceneDetailsSO sceneDetailsSo)
        {
            _sceneDetails = sceneDetailsSo;
            _sceneAssetsToLoad.Clear();
            _sceneNamesToUnload.Clear();
            _sceneAssetsToLoad.Add(sceneDetailsSo.sceneAsset);
            _sceneNamesToUnload.Add(sceneDetailsSo.sceneName);
        }

        private void ResetPlayerWhenRespawn()
        {
            if (_isPlayerDeath)
            {
                _isPlayerDeath = false;
                playerGO.transform.localPosition = Vector3.zero;
                if (playerGO.transform.localRotation != Quaternion.identity && _currentDefaultFacingDirection == 1)
                {
                    onPlayerFlip.Broadcast();
                }
                else if (playerGO.transform.localRotation == Quaternion.identity && _currentDefaultFacingDirection == -1)
                {
                    onPlayerFlip.Broadcast();
                }
                onPlayerRespawn.Broadcast();
                ResetHealth();
                if (resetManaWhenRespawn)
                {
                    ResetMana();
                }
             
            }
        }

        private void ResetChildrenTransform()
        {
            // playerGO.transform.localRotation = Quaternion.identity;
            // playerGO.transform.localScale = Vector3.one;
            playerGO.transform.localPosition = Vector3.zero;
        }

        // public void FreezePlayerY()
        // {
        //     _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        // }
        //
        // public void UnfreezePlayerY()
        // {
        //     _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        // }
        
        public Transform GetPlayerHolderTransform()
        {
            return playerGO.transform;
        }

        private void ResetHealth()
        {
            playerHealth.Variable.SetValue(playerMaxHealth);
            onPlayerHealthChange.Broadcast(playerHealth.Value);
        }
        
        private void ResetMana()
        {
            playerMana.Variable.SetValue(playerMaxMana);
            onPlayerManaChange.Broadcast(playerMana.Value);
        }
        
        private void OnPlayerDeath()
        {
            _isPlayerDeath = true;
            SceneLoader.LoadAddressableScene(_sceneDetails.sceneAsset,false);
        }
        
        

        #region w/ Unity Functins

        private void Start()
        {
            playerGO.TryGetComponent(out _rigidbody);
            
            CameraManager.Instance.MainCamera.GetUniversalAdditionalCameraData().volumeTrigger =
                GetPlayerHolderTransform();
            
            SetCurrentRespawnScene(initialSceneData);
        }

        private void OnEnable()
        {
            onPlayerDeath.AddListener(OnPlayerDeath);
            SceneLoader.ActivatingLoadedScene += ResetPlayerWhenRespawn;
        }

        private void OnDisable()
        {
            onPlayerDeath.RemoveListener(OnPlayerDeath);
            SceneLoader.ActivatingLoadedScene -= ResetPlayerWhenRespawn;
        }

        #endregion
        

    }
}