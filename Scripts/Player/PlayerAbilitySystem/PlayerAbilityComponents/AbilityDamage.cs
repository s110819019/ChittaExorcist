using System;
using ChittaExorcist.CharacterCore;
using UnityEngine;

using ChittaExorcist.Common.Interfaces;
using ChittaExorcist.GameCore.AudioSettings;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    public class AbilityDamage : PlayerAbilityComponent<AbilityDamageData, AbilityDamagePhaseData>
    {
        #region w/ Ability Components

        private AbilityActionHitBox _hitBox;

        #endregion

        #region w/ Core Comp

        private Movement _coreMovement;

        #endregion

        #region w/ Events

        // 攻擊特效生成位置使用
        public event Action<Transform> OnAttack;

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

        #region w/ Damage

        private void HandleDetectedCollider(Collider2D[] collider2Ds)
        {
            foreach (var item in collider2Ds)
            {
                if (item.TryGetComponent(out IDamageable damageable))
                {
                    damageable.Damage(CurrentPhaseData.Amount);
                    OnAttack?.Invoke(item.transform);
                    CheckManaRecover();
                    // TODO :有 NULL問題
                    if (CurrentPhaseData.AudioData != null)
                    {
                        AudioManager.Instance.PlayOnceAudio(CurrentPhaseData.AudioData);
                    }
                    // Debug.Log($"Damage! {CurrentPhaseData.Amount} Amount");
                }

                if (item.TryGetComponent(out IDestructible destructible))
                {
                    destructible.Damage(_coreMovement.FacingDirection);
                    // OnAttack?.Invoke(item.transform);
                    // CheckManaRecover();
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

            Core.GetCoreComponent(out _coreMovement);
        }

        #endregion
    }
}