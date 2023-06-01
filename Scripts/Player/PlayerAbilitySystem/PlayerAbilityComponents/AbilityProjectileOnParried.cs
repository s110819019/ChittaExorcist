using System;
using UnityEngine;
using ChittaExorcist.CharacterCore;
using ChittaExorcist.Common.Interfaces;
using ChittaExorcist.Structs;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityProjectileOnParried : PlayerAbilityComponent<AbilityProjectileOnParriedData, AbilityProjectileOnParriedPhaseData>
    {
        #region w/ Ability Components

        private AbilityParry _parry;

        private ParriedDetails _parriedDetails;

        #endregion
        
        #region w/ Events

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            _parry.OnDetectedCollider2D += HandleDetectedCollider;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            if (_parry)
            {
                _parry.OnDetectedCollider2D -= HandleDetectedCollider;
            }
        }

        #endregion

        #region w/ Core Components

        #endregion

        #region w/ ProjectileOnParried

        private void HandleDetectedCollider(Collider2D[] collider2Ds)
        {
            foreach (var item in collider2Ds)
            {
                if (item.TryGetComponent(out IParryable parryable))
                {
                    _parriedDetails.IsSetParriedProjectile = true;
                    
                    _parriedDetails.ParriedProjectileDetails.InteractableLayers = ComponentData.ProjectileDetectableLayers;
                    _parriedDetails.ParriedProjectileDetails.TravelTime = CurrentPhaseData.TravelTime;
                    _parriedDetails.ParriedProjectileDetails.SpeedCurve = CurrentPhaseData.SpeedCurve;
                    
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