using System;
using UnityEngine;
using ChittaExorcist.CharacterCore;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityDamageEffect : PlayerAbilityComponent<AbilityDamageEffectData, AbilityDamageEffectPhaseData>
    {
        #region w/ Ability Components

        private AbilityDamage _damage;

        #endregion
        
        #region w/ Events

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            _damage.OnAttack += HandleAttack;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            _damage.OnAttack -= HandleAttack;
        }

        #endregion

        #region w/ Core Components

        private CoreComp<ParticleManager> _particleManager;

        #endregion

        #region w/ DamageEffect

        private void HandleAttack(Transform spawnTransform)
        {
            if (CurrentPhaseData.Particle == null)
            {
                Debug.Log("No Effect Set up");
                return;
            }

            if (CurrentPhaseData.UseRandomRotate)
            {
                _particleManager.Comp.GetParticleWithRotate(CurrentPhaseData.Particle, CurrentPhaseData.Offset,
                    spawnTransform, CurrentPhaseData.MinAngle, CurrentPhaseData.MaxAngle);
            }
            else
            {
                _particleManager.Comp.GetParticle(CurrentPhaseData.Particle, CurrentPhaseData.Offset, spawnTransform);
            }
        }
        
        #endregion
        
        #region w/ Unity Callback Functions

        protected override void Start()
        {
            _particleManager = new CoreComp<ParticleManager>(Core);
            _damage = GetComponent<AbilityDamage>();
            base.Start();
        }

        #endregion
    }
}