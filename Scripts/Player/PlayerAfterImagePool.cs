using System;
using System.Collections.Generic;
using UnityEngine;

using ChittaExorcist.PlayerSettings.FSM;

namespace ChittaExorcist.PlayerSettings
{
    public class PlayerAfterImagePool : MonoBehaviour
    {
        [SerializeField] private GameObject afterImagePrefab;
        [SerializeField] private Player player;

        public Player Player
        {
            get => player;
            private set => player = value;
        }

        private readonly Queue<GameObject> _availableObjects = new Queue<GameObject>();
    
        public static PlayerAfterImagePool Instance { get; private set; }

        #region w/ Unity Callback Funcitons

        private void Awake()
        {
            if (Instance != null)
            {
                return;
            }
            Instance = this;
            GrowPool();
        }

        #endregion
    
        public void AddToPool(GameObject instance)
        {
            instance.SetActive(false);
            _availableObjects.Enqueue(instance);
        }
    
        private void GrowPool()
        {
            for (int i = 0; i < 10; i++)
            {
                var instanceToAdd = Instantiate(afterImagePrefab, transform, true);
                AddToPool(instanceToAdd);
            }
        }

        public GameObject GetFromPool()
        {
            if (_availableObjects.Count == 0)
            {
                GrowPool();
            }

            var instance = _availableObjects.Dequeue();
            instance.SetActive(true);
            return instance;
        }
    }
}