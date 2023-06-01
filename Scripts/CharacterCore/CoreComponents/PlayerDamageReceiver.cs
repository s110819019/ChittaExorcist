using System;
using ChittaExorcist.Common.Interfaces;
using ChittaExorcist.GameCore;
using ChittaExorcist.PlayerSettings.PlayerAbilitySystem;
using UnityEngine;

namespace ChittaExorcist.CharacterCore
{
    public class PlayerDamageReceiver : CoreComponent, IDamageable
    {
        [SerializeField] private float damageOpacityTime = 2.0f;
        
        public event Action<float> OnDamage;
        public event Action OnHit;
        public event Action<bool> OnDamageOpacity; 
        // public event Action<float> OnStartDamageOpacity;
        // public event Action OnStopDamageOpacity;

        #region w/ Variables

        private float _damageOpacityStartTime;
        private bool _isDamageOpacity;

        #endregion

        #region w/ Component

        private Collider2D _collider;

        #endregion
        
        #region w/ Player Ability

        private AbilityBlock _abilityBlock;

        public void InitializeAbilityBlock(AbilityBlock block)
        {
            _abilityBlock = block;
        }

        private AbilityParry _abilityParry;

        public void InitializeAbilityParry(AbilityParry parry)
        {
            _abilityParry = parry;
        }

        private bool _isBlockSuccess;
        private bool _isParrySuccess;
        
        #endregion

        #region w/ Events

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
        }

        #endregion
        
        #region w/ Damage Interface

        public void Damage(float amount)
        {
            OnDamage?.Invoke(amount);
            if (_isBlockSuccess || _isParrySuccess)
            {
                _isBlockSuccess = _isParrySuccess = false;
                return;
            }
            OnHit?.Invoke();
        }

        public void Damage(float amount, IParryable parryable)
        {
            if (_isDamageOpacity)
            {
                Debug.LogWarning("Why Player Get Damage When DamageOpacity ?");
            }
            
            if (parryable != null)
            {
                if (parryable.IsSceneTrap)
                {
                    Damage(amount);
                    StartOpacityFlash();
                    return;
                }
                
                if (_abilityParry != null)
                {
                    // if (_abilityParry.IsParrySuccess)
                    // {
                    //     Damage(_abilityParry.CheckDamageAbsorption(amount));
                    //     return;
                    // }
                    if (parryable.IsParried)
                    {
                        _isParrySuccess = true;
                        _abilityParry.GetDamage();
                        Damage(_abilityParry.CheckDamageAbsorption(amount));
                        return;
                    }
                }
                
                if (_abilityBlock != null)
                {
                    // Block Success
                    if (_abilityBlock.CheckPlayerGetDamagedBlockSuccess(parryable.AttackTransform))
                    {
                        _isBlockSuccess = true;
                        _abilityBlock.GetDamage();
                        Damage(_abilityBlock.CheckDamageAbsorption(amount));
                        return;
                    }
                }                
            }
            
            Damage(amount);
        }        

        #endregion

        private void StartOpacityFlash()
        {
            // OnStartDamageOpacity?.Invoke(damageOpacityTime);
            OnDamageOpacity?.Invoke(true);
            _damageOpacityStartTime = Time.time;
            _isDamageOpacity = true;
            DisableHitBox();
        }

        private void StopOpacityFlash()
        {
            OnDamageOpacity?.Invoke(false);
            _isDamageOpacity = false;
            EnableHitBox();
        }

        private void CheckDamageOpacity()
        {
            if (!_isDamageOpacity) return;
            if (Time.time >= _damageOpacityStartTime + damageOpacityTime)
            {
                StopOpacityFlash();
            }
        }

        public void EnableHitBox()
        {
            if (_collider == null)
            {
                Debug.LogWarning("Try to enable a collider not exist !");
                return;
            }

            _collider.enabled = true;
        }
        
        public void DisableHitBox()
        {
            if (_collider == null)
            {
                Debug.LogWarning("Try to disable a collider not exist !");
                return;
            }

            _collider.enabled = false;
        }
        
        #region w/ Unity Callback Function

        protected override void Awake()
        {
            base.Awake();
            
            // Stats Comp
            // Particle Comp
            TryGetComponent(out _collider);
            if (_collider == null)
            {
                Debug.LogWarning("No Collider 2D on PlayerDamageReceiver");
            }
        }

        private void Update()
        {
            CheckDamageOpacity();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            EnableHitBox();
        }

        #endregion
    }
}