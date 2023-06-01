using System;
using UnityEngine;

using ChittaExorcist.CharacterCore;
using ChittaExorcist.Common;
using ChittaExorcist.Common.Interfaces;
using ChittaExorcist.GameCore;
using ChittaExorcist.Structs;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityRanged : PlayerAbilityComponent<AbilityRangedData, AbilityRangedPhaseData>
    {
        #region w/ Events

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            EventHandler.OnRanged += HandleRanged;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            EventHandler.OnRanged -= HandleRanged;
        }

        #endregion

        #region w/ Variables

        private ProjectileDetails _projectileDetails;
        private Vector2 _workspace;

        #endregion

        #region w/ Component
        

        #endregion

        #region w/ Core Components

        private Movement _coreMovement;
        private Movement CoreMovement => _coreMovement ? _coreMovement : Core.GetCoreComponent(out _coreMovement);

        // Player Mana
        protected PlayerManaStats PlayerManaStats => _playerManaStats ? _playerManaStats : Core.GetCoreComponent(out _playerManaStats);
        private PlayerManaStats _playerManaStats;
        
        #endregion

        #region w/ Ranged

        private void HandleRanged()
        {
            if (CoreMovement == null)
            {
                Debug.LogWarning("無法取得 Movement Core");
                // Debug.LogWarning($"Core 目前是否為 NULL : {Core == null}");
            }
            
            // if (PlayerManaStats == null)
            // {
            //     Debug.LogWarning("無法取得 PlayerManaStats Core");
            // }

            if (!CheckManaRequire())
            {
                return;
            }
            
            // Debug.Log(CurrentPhaseData);

            _workspace.Set(transform.position.x + (CurrentPhaseData.StartPosition.x * _coreMovement.FacingDirection),
                transform.position.y + CurrentPhaseData.StartPosition.y);
            // 起始位置
            _projectileDetails.StartPosition = _workspace;

            _workspace.Set(transform.right.x * CurrentPhaseData.StartDirection.x, CurrentPhaseData.StartDirection.y);
            // 起始角度
            _projectileDetails.StartDirection = _workspace;

            // 移動時間
            _projectileDetails.TravelTime = CurrentPhaseData.TravelTime;
            
            // 速度曲線
            _projectileDetails.SpeedCurve = CurrentPhaseData.SpeedCurve;
            
            // 翻轉 ?
            _projectileDetails.ShouldFlip = _coreMovement.FacingDirection == -1;

            // == Combat ==
            _projectileDetails.Damage = CurrentPhaseData.Damage;
            _projectileDetails.PoiseDamage = CurrentPhaseData.PoiseDamage;
            _projectileDetails.KnockbackStrength = CurrentPhaseData.KnockbackStrength;
            _projectileDetails.KnockbackDirection = CurrentPhaseData.KnockbackDirection;

            // 使用重力 ?
            _projectileDetails.UseFallGravity = CurrentPhaseData.UseFallGravity;

            // TODO: 如何設定取得 player transform ?
            // 使用者位置
            _projectileDetails.ProjectileUser = transform;

            // 可互動圖層
            _projectileDetails.InteractableLayers = ComponentData.DetectableLayers;


            // == Projectile Settings ==
            var projectile = ObjectPoolManager.Instance.GetObject(CurrentPhaseData.ProjectilePrefab);
            var projectileScript = projectile.GetComponent<IProjectile>();

            if (projectileScript != null)
            {
                StartProjectile(projectileScript);
            }
            else
            {
                Debug.LogError(" Projectile does not have the right script attached ");
            }
        }

        #endregion

        #region w/ Projectile

        private void StartProjectile(IProjectile script)
        {
            script.SetProjectileStartingState(_projectileDetails);
        }

        #endregion

        #region w/ Unity Callback Functions

        protected override void Start()
        {
            base.Start();
        }

        #endregion
    }
}