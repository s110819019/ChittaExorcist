using System;
using UnityEngine;
using ChittaExorcist.CharacterCore;
using ChittaExorcist.Common.Interfaces;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityKnockback : PlayerAbilityComponent<AbilityKnockbackData, AbilityKnockbackPhaseData>
    {
        #region w/ Ability Components

        private AbilityActionHitBox _hitBox;

        #endregion
        
        #region w/ Events

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            _hitBox.OnDetectedCollider2D += HandleDetectedCollider;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            _hitBox.OnDetectedCollider2D -= HandleDetectedCollider;
        }

        #endregion

        #region w/ Core Components

        private Movement _coreMovement;
        private Movement CoreMovement => _coreMovement ? _coreMovement : Core.GetCoreComponent(out _coreMovement);
        
        #endregion

        #region w/ Knockback

        private void HandleDetectedCollider(Collider2D[] collider2Ds)
        {
            if (CoreMovement == null)
            {
                Debug.LogWarning("無法取得 Movement Core");
                // Debug.LogWarning($"Core 目前是否為 NULL : {Core == null}");
            }
            
            foreach (var item in collider2Ds)
            {
                if (item.TryGetComponent(out IKnockbackable knockbackable))
                {
                    knockbackable.Knockback(CurrentPhaseData.KnockbackAngle, CurrentPhaseData.KnockbackStrength, CoreMovement.FacingDirection);
                    // Debug.Log($"Damage! {CurrentPhaseData.Amount} Amount");
                }
            }
        }
        
        #endregion
        
        #region w/ Unity Callback Functions

        protected override void Start()
        {
            // 要先取得 HitBox
            _hitBox = GetComponent<AbilityActionHitBox>();
            base.Start();
        }

        #endregion
    }
}