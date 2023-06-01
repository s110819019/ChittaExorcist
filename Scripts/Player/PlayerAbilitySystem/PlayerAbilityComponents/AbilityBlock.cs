using System;
using ChittaExorcist.CharacterCore;
using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityBlock : PlayerAbilityComponent<AbilityBlockData, AbilityBlockPhaseData>
    {
        #region w/ Events
    
        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            EventHandler.OnStartBlock += HandleStartBlock;
            EventHandler.OnStopBlock += HandleStopBlock;
            // AbilityEventChannelHolder.onPlayerGetDamagedWithTransform.AddListener(OnPlayerGetDamagedWithTransform);
        }
    
        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            EventHandler.OnStartBlock -= HandleStartBlock;
            EventHandler.OnStopBlock -= HandleStopBlock;
        }
    
        #endregion

        public bool CheckPlayerGetDamagedBlockSuccess(Transform attackTransform)
        {
            if (!_isBlockActive) return false;

            var attackVector = attackTransform.position - transform.position;
            var attackAngle = Vector3.Angle(attackVector, transform.right);
            var isNegative = attackTransform.position.y < transform.position.y;

            attackAngle = isNegative ? -attackAngle : attackAngle;
            
            
            foreach (var item in CurrentPhaseData.BlockDirections)
            {
                if (item.MaxAngle < 0 && item.MinAngle > 0)
                {
                    if (attackAngle <= item.MaxAngle && (360.0f + attackAngle) >= item.MinAngle)
                    {
                        return true;
                    }
                }
                else
                {
                    if (attackAngle <= item.MaxAngle && attackAngle >= item.MinAngle)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public float CheckDamageAbsorption(float amount)
        {
            return (int) (amount - (amount * CurrentPhaseData.DamageAbsorption));
        }
        
        public float CheckKnockbackStrengthAbsorption(float amount)
        {
            return (int) (amount - (amount * CurrentPhaseData.KnockbackStrengthAbsorption));
        }

        #region w/ Variables

        private bool _isBlockActive;

        #endregion
        
        #region w/ Core Components

        // Movement
        private Movement _coreMovement;
        private Movement CoreMovement => _coreMovement ? _coreMovement : Core.GetCoreComponent(out _coreMovement);

        // Player Damage Receiver
        private PlayerDamageReceiver _corePlayerDamageReceiver;
        private PlayerDamageReceiver CorePlayerDamageReceiver => _corePlayerDamageReceiver
            ? _corePlayerDamageReceiver
            : Core.GetCoreComponent(out _corePlayerDamageReceiver);

        // Player Knockback Receiver
        private PlayerKnockbackReceiver _corePlayerKnockbackReceiver;
        private PlayerKnockbackReceiver CorePlayerKnockbackReceiver => _corePlayerKnockbackReceiver
            ? _corePlayerKnockbackReceiver
            : Core.GetCoreComponent(out _corePlayerKnockbackReceiver);

        private ParticleManager _coreParticleManager;

        #endregion

        #region w/ Workflow

        protected override void HandleEnter()
        {
            base.HandleEnter();
        }

        protected override void HandleExit()
        {
            base.HandleExit();
            _isBlockActive = false;
            Ability.Animator.SetBool("getDamage", false);
        }

        #endregion

        #region w/ Block

        private void HandleStartBlock()
        {
            _isBlockActive = true;
            CorePlayerDamageReceiver.InitializeAbilityBlock(this);
            CorePlayerKnockbackReceiver.InitializeAbilityBlock(this);
        }

        private void HandleStopBlock()
        {
            _isBlockActive = false;
            CorePlayerDamageReceiver.InitializeAbilityBlock(null);
            CorePlayerKnockbackReceiver.InitializeAbilityBlock(null);
        }

        public void GetDamage()
        {
            if (CurrentPhaseData.AudioData != null)
            {
                AudioManager.Instance.PlayOnceAudio(CurrentPhaseData.AudioData);
            }
            Ability.Animator.SetBool("getDamage", true);
            if (CurrentPhaseData.BlockParticle != null)
            {
                _coreParticleManager.GetParticle(CurrentPhaseData.BlockParticle, CurrentPhaseData.BlockParticleOffset);
            }
        }
        
        #endregion

        #region w/ Unity Callback Function

        protected override void Start()
        {
            base.Start();

            Core.GetCoreComponent(out _coreParticleManager);
        }

        private void Update()
        {
            if (IsPhaseActive)
            {
                CoreMovement.SetVelocityX(0.0f);
            }
        }        

        #endregion
        

    }
}
