
using ChittaExorcist.CharacterCore;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    public class AbilityActionEffect : PlayerAbilityComponent<AbilityActionEffectData, AbilityActionEffectPhaseData>
    {
        #region w/ Events

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            EventHandler.OnActionEffect += HandleActionEffect;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            EventHandler.OnActionEffect -= HandleActionEffect;
        }

        #endregion

        #region w/ Core Components

        private CoreComp<ParticleManager> _particleManager;

        #endregion
        
        #region w/ Action Effect

        private void HandleActionEffect()
        {
            if (CurrentPhaseData.Particle == null)
            {
                Debug.Log("No Effect Set up");
                return;
            }
            _particleManager.Comp.GetParticle(CurrentPhaseData.Particle, CurrentPhaseData.Offset);
        }

        #endregion
        
        #region w/ Unity Callback Functions

        protected override void Start()
        {
            base.Start();
            _particleManager = new CoreComp<ParticleManager>(Core);
        }

        #endregion
    }
}