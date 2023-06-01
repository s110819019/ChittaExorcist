using UnityEngine;

using ChittaExorcist.CharacterCore;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    public class AbilityMovement : PlayerAbilityComponent<AbilityMovementData, AbilityMovementPhaseData>
    {
        #region w/ Events

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            EventHandler.OnStartMovement += HandleStartMovement;
            EventHandler.OnStopMovement += HandleStopMovement;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            EventHandler.OnStartMovement -= HandleStartMovement;
            EventHandler.OnStopMovement -= HandleStopMovement;
        }

        #endregion
        
        #region w/ Core Components

        private Movement _coreMovement;
        private Movement CoreMovement => _coreMovement ? _coreMovement : Core.GetCoreComponent(out _coreMovement);

        #endregion

        #region w/ Movement

        private void HandleStartMovement()
        {
            if (CoreMovement == null)
            {
                Debug.LogWarning("無法取得 Movement Core");
                // Debug.LogWarning($"Core 目前是否為 NULL : {Core == null}");
            }
            
            CoreMovement.SetVelocity(CurrentPhaseData.Velocity, CurrentPhaseData.Direction, _coreMovement.FacingDirection);
        }

        private void HandleStopMovement()
        {
            if (CoreMovement == null)
            {
                Debug.LogWarning("無法取得 Movement Core");
            }
            
            CoreMovement.SetVelocityZero();
        }        

        #endregion
    }
}