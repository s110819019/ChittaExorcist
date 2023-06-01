using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using ChittaExorcist.CharacterCore;
using ChittaExorcist.Common.Interfaces;
using ChittaExorcist.GameCore;
using ChittaExorcist.GameCore.AudioSettings;
using ChittaExorcist.Structs;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityParry : PlayerAbilityComponent<AbilityParryData, AbilityParryPhaseData>
    {
        #region w/ Events

        public event Action<Collider2D[]> OnDetectedCollider2D;

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            EventHandler.OnStartParry += HandleStartParry;
            EventHandler.OnStopParry += HandleStopParry;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            EventHandler.OnStartParry -= HandleStartParry;
            EventHandler.OnStopParry -= HandleStopParry;
        }

        #endregion

        #region w/ Core Components

        // private CoreComp<Movement> _movement;
        private Movement _coreMovement;
        private PlayerDamageReceiver _corePlayerDamageReceiver;
        private PlayerKnockbackReceiver _corePlayerKnockbackReceiver;
        private ParticleManager _coreParticleManager;

        #endregion
        
        #region w/ Variables

        private Vector2 _offset;
        private Collider2D[] _detected;
        private bool _isParryActive;
        private bool _isParrySuccess;

        // public bool IsParrySuccess => _isParrySuccess;

        private float _lastCameraShakeStrength;
        private float _lastCameraShakeFrequency;
        private float _lastCameraShakeDuration;

        private float _lastParryMaxDelayTime;
        
        private float _parryStartTime;

        // private ProjectileGetParriedDetails _projectileGetParriedDetails;

        #endregion
        
        #region w/ Parry

        private void HandleStartParry()
        {
            _isParryActive = true;
            _isParrySuccess = false;
            
            _corePlayerDamageReceiver.InitializeAbilityParry(this);
            _corePlayerKnockbackReceiver.InitializeAbilityParry(this);
            // Debug.Log("Start Parry Check");
        }

        private void HandleStopParry()
        {
            _isParryActive = false;

            // CorePlayerDamageReceiver.InitializeAbilityParry(null);
            // CorePlayerKnockbackReceiver.InitializeAbilityParry(null);
            
            // Debug.Log("Exit Parry Check");
        }

        public void CheckParryHitBox()
        {
            if (!_isParryActive || _isParrySuccess || !IsPhaseActive) return;
            
            // Parry Box
            _offset.Set(transform.position.x + (CurrentPhaseData.ParryHitBox.center.x * _coreMovement.FacingDirection),
                transform.position.y + CurrentPhaseData.ParryHitBox.center.y);
        
            // Detected Obj
            _detected = Physics2D.OverlapBoxAll(_offset, CurrentPhaseData.ParryHitBox.size, 0.0f,
                ComponentData.DetectableLayers);
        
            if (_detected.Length == 0)
            {
                return;
            }
        
            foreach (var item in _detected)
            {
                if (item.TryGetComponent(out IParryable parryable))
                {
                    parryable.Parry();
                    Ability.Animator.SetBool("parry", true);
                    // Debug.Log("Parry Sus");

                    _parryStartTime = Time.time;
                    _isParrySuccess = true;
                }
            }
            
            OnDetectedCollider2D?.Invoke(_detected);
        }

        public void GetDamage()
        {
            // Do Camera Shake
            CameraManager.Instance.ShakeCamera(_lastCameraShakeStrength, _lastCameraShakeFrequency, _lastCameraShakeDuration);

            // Change to parry animation
            // when "parry" and "getDamage" equal ture
            AudioManager.Instance.PlayOnceAudio(CurrentPhaseData.AudioData);
            Ability.Animator.SetBool("getDamage", true);
            if (CurrentPhaseData.ParryParticle != null)
            {
                _coreParticleManager.GetParticle(CurrentPhaseData.ParryParticle, CurrentPhaseData.ParryParticleOffset);
            }
        }

        public void CheckParryTimeExceeded()
        {
            if (!_isParrySuccess) return;
            
            // 在 parry 成功，但未確實收到敵人攻擊並呼叫 GetDamage 時，判斷是否超過時間以取消動作
            if (Time.time >= _parryStartTime + _lastParryMaxDelayTime)
            {
                // Change to parry animation
                // when "timeExceed" equal ture
                Ability.Animator.SetBool("timeExceed", true);
            }
        }
        
        public float CheckDamageAbsorption(float amount)
        {
            return (int) (amount - (amount * CurrentPhaseData.DamageAbsorption));
        }

        public float CheckKnockbackStrengthAbsorption(float amount)
        {
            return (int) (amount - (amount * CurrentPhaseData.KnockbackStrengthAbsorption));
        }
        
        #endregion

        #region w/ Draw Gizmos

        private void OnDrawGizmosSelected()
        {
            if (ComponentData == null) return;

            foreach (var item in ComponentData.PhaseData)
            {
                if (!item.debug)
                {
                    continue;
                }

                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(transform.position + (Vector3) item.ParryHitBox.center, item.ParryHitBox.size);
            }
        }

        #endregion

        #region w/ Workflow

        protected override void HandleEnter()
        {
            base.HandleEnter();
            
            _lastCameraShakeStrength = CurrentPhaseData.CameraShakeStrength;
            _lastCameraShakeFrequency = CurrentPhaseData.CameraShakeFrequency;
            _lastCameraShakeDuration = CurrentPhaseData.CameraShakeDuration;
            _lastParryMaxDelayTime = CurrentPhaseData.ParryMaxDelayTime;
            
            Ability.Animator.SetBool("parry", false);
        }

        protected override void HandleExit()
        {
            base.HandleExit();
            _isParryActive = false;
            _isParrySuccess = false;
            
            Ability.Animator.SetBool("parry", false);
            Ability.Animator.SetBool("getDamage", false);
            Ability.Animator.SetBool("timeExceed", false);
        }

        #endregion
        
        #region w/ Unity Callback Functions

        protected override void Start()
        {
            base.Start();
            
            Core.GetCoreComponent(out _coreMovement);
            Core.GetCoreComponent(out _corePlayerDamageReceiver);
            Core.GetCoreComponent(out _corePlayerKnockbackReceiver);
            Core.GetCoreComponent(out _coreParticleManager);
        }

        private void Update()
        {
            // Debug.Log("IsParryActive : " + _isParryActive);
            CheckParryHitBox();
            CheckParryTimeExceeded();
        }

        #endregion
    }
}