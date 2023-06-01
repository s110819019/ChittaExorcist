using System;
using UnityEngine;

using ChittaExorcist.Common.Interfaces;
using ChittaExorcist.EventChannel;

namespace ChittaExorcist.CharacterCore
{
    public class DamageReceiver : CoreComponent, IDamageable
    {
        // [SerializeField] private BoolEventChannel onPlayerSuccessBlock;
        
        public event Action<float> OnDamage;
        public event Action OnHit;

        #region w/ Events

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            // onPlayerSuccessBlock?.AddListener(OnPlayerSuccessBlock);
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            // onPlayerSuccessBlock?.RemoveListener(OnPlayerSuccessBlock);
        }

        #endregion
        
        #region w/ Damage Interface

        public void Damage(float amount)
        {
            // Debug.Log(Core.transform.parent.name + "Damaged!");
            
            // TODO: 減少生命值與生成特效?

            // if (_isBlock && onPlayerSuccessBlock)
            // {
            //     _isBlock = false;
            //     return;
            // }
            
            OnHit?.Invoke();

            OnDamage?.Invoke(amount);
        }

        public void Damage(float amount, Transform attackTransform)
        {
            
        }

        public void Damage(float amount, IParryable parryable)
        {
            
        }
        
        #endregion

        #region w/ Unity Callback Function

        protected override void Awake()
        {
            base.Awake();
            
            // Stats Comp
            // Particle Comp
        }

        private void Update()
        {
            // Debug.Log(_isBlock);
        }

        #endregion
    }
}