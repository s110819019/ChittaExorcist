using System;
using UnityEngine;

using ChittaExorcist.EventChannel;
using ChittaExorcist.PlayerEffectSettings;

namespace ChittaExorcist.PlayerSettings
{
    public class PlayerHolder : MonoBehaviour
    {
        [Header("Broadcast and Listen Event Channel")]
        [SerializeField] public IntEventChannel onPlayerSwitch;
        
        [Header("Broadcast Event Channel")]
        [SerializeField] public VoidEventChannel onPlayerDeath;

        [Header("Listen Event Channel")]
        [SerializeField] public VoidEventChannel onPlayerRespawn;
        [SerializeField] public VoidEventChannel onPlayerFlip;
        [SerializeField] public VoidEventChannel onPlayerCanSwitch;

        [Header("Player Effect")]
        [SerializeField] private PlayerEffect playerSwitchEffect;
        [SerializeField] private PlayerEffect playerSwitchEffect2;
        [SerializeField] private PlayerEffect playerChargeEffect;
        [SerializeField] private PlayerEffect playerChargeDoneEffect;
        [SerializeField] private PlayerEffect playerJumpSmokeEffect;
        [SerializeField] private PlayerEffect playerWalkSmokeEffect;

        [Header("Player Particle")]
        [SerializeField] private ParticleSystem playerWalkEffect;
        [SerializeField] private ParticleSystem playerJumpEffect;
        [SerializeField] private ParticleSystem playerLandEffect;

        public bool CanSwitchCharacter { get; private set; }

        public void SetCanSwitchCharacter()
        {
            CanSwitchCharacter = true;
        }
        
        #region w/ Event Subscribe
        
        // 訂閱事件
        protected virtual void SetSubscribeEvents()
        {
            onPlayerSwitch.AddListener(PlayPlayerSwitchEffect);
            onPlayerSwitch.AddListener(PlayPlayerSwitchEffect2);
            if (onPlayerCanSwitch)
            {
                onPlayerCanSwitch.AddListener(SetCanSwitchCharacter);
            }
        }
        
        // 取消訂閱事件
        protected virtual void SetUnsubscribeEvents()
        {
            onPlayerSwitch.RemoveListener(PlayPlayerSwitchEffect);
            onPlayerSwitch.RemoveListener(PlayPlayerSwitchEffect2);
            if (onPlayerCanSwitch)
            {
                onPlayerCanSwitch.RemoveListener(SetCanSwitchCharacter);
            }
        }
        
        #endregion

        #region w/ Player Effect

        // Switch Effect
        private void PlayPlayerSwitchEffect(int valueHereNoUse)
        {
            playerSwitchEffect.PlayAnimationFromZero("Start");
        }
        
        private void PlayPlayerSwitchEffect2(int valueHereNoUse)
        {
            playerSwitchEffect2.PlayAnimationFromZero("Start");
        }

        // Charge
        public void PlayPlayerChargeEffectLoop()
        {
            playerChargeEffect.PlayAnimation("Loop");
        }

        public void StopPlayPlayerChargeEffect()
        {
            playerChargeEffect.PlayEmpty();
        }
        
        // Charge Done
        public void PlayerPlayerChargeDoneEffectEnd()
        {
            playerChargeDoneEffect.PlayAnimationFromZero("End");
        }

        #endregion

        public void PlayPlayerJumpSmokeEffect()
        {
            playerJumpSmokeEffect.PlayAnimationFromZero("Start");
        }

        public void PlayPlayerWalkSmokeEffectLoop()
        {
            playerWalkSmokeEffect.PlayAnimation("Loop");
        }
        
        public void StopPlayPlayerWalkSmokeEffect()
        {
            playerWalkSmokeEffect.PlayEmpty();
        }
        
        #region w/ Player Particle

        public void PlayPlayerWalkParticle(bool value)
        {
            // playerWalkEffect.gameObject.SetActive(value);
            if (value)
            {
                playerWalkEffect.Play();
            }
            else
            {
                playerWalkEffect.Stop();
            }
        }

        public void PlayPlayerJumpParticle(bool value)
        {
            if (value)
            {
                playerJumpEffect.Play();
            }
            else
            {
                playerJumpEffect.Stop();
            }
        }
        
        public void PlayPlayerLandParticle(bool value)
        {
            if (value)
            {
                playerLandEffect.Play();
            }
            else
            {
                playerLandEffect.Stop();
            }
        }

        #endregion
        
        #region w/ Unity Callback Functions

        private void Awake()
        {
            if (onPlayerCanSwitch == null)
            {
                CanSwitchCharacter = true;
            }
        }

        private void OnEnable()
        {
            SetSubscribeEvents();
        }

        private void OnDisable()
        {
            SetUnsubscribeEvents();
        }

        #endregion
    }
}