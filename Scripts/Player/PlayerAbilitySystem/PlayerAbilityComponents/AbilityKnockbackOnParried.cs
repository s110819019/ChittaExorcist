using System;
using UnityEngine;
using ChittaExorcist.CharacterCore;
using ChittaExorcist.Common.Interfaces;
using ChittaExorcist.Structs;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityKnockbackOnParried : PlayerAbilityComponent<AbilityKnockbackOnParriedData, AbilityKnockbackOnParriedPhaseData>
    {
        #region w/ Ability Components

        private AbilityParry _parry;

        private ParriedDetails _parriedDetails;

        #endregion

        #region w/ Events

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            // Debug.Log("Parry start: " + _parry + "is" + transform.parent.transform.parent.name);
            if (_parry)
            {
                _parry.OnDetectedCollider2D += HandleDetectedCollider;
            }
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            // Debug.Log("Parry exit: " + _parry + "is" + transform.parent.transform.parent.name);
            if (_parry)
            {
                _parry.OnDetectedCollider2D -= HandleDetectedCollider;
            }
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
                if (item.TryGetComponent(out IParryable parryable))
                {
                    _parriedDetails.IsSetParriedKnockback = true;
                    
                    _parriedDetails.ParriedKnockbackDetails.KnockbackAngle = CurrentPhaseData.KnockbackAngle;
                    _parriedDetails.ParriedKnockbackDetails.KnockbackStrength = CurrentPhaseData.KnockbackStrength;
                    _parriedDetails.ParriedKnockbackDetails.KnockbackDirection = CoreMovement.FacingDirection;
                    
                    parryable.CheckParryDetails(_parriedDetails);
                }
            }
        }

        #endregion

        #region w/ Unity Callback Functions

        protected override void Start()
        {
            _parry = GetComponent<AbilityParry>();
            base.Start();
        }

        #endregion
    }
}