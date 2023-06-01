using ChittaExorcist.Common.Interfaces;
using ChittaExorcist.GameCore;
using ChittaExorcist.Structs;
using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E_RangedAttackState<T1> : E_AttackState<T1, ED_RangedAttackStateSO> where T1 : Enemy
    {
        public E_RangedAttackState(string animationBoolName, T1 enemy, ED_RangedAttackStateSO stateData, Transform attackPosition) : base(animationBoolName, enemy, stateData, attackPosition)
        {
            EventHandler.OnRangedAttack += TriggerAttack;
        }

        #region w/ Projectile

        private Vector2 _workspace;
        private ProjectileDetails _projectileDetails;

        private void InitializeProjectileData()
        {
            _workspace.Set(Enemy.transform.position.x + (StateData.startPosition.x * Movement.FacingDirection),
                Enemy.transform.position.y + StateData.startPosition.y);
            // 起始位置
            _projectileDetails.StartPosition = _workspace;

            _workspace.Set(Enemy.transform.right.x * StateData.startDirection.x, StateData.startDirection.y);
            // 起始角度
            _projectileDetails.StartDirection = _workspace;

            // 移動時間
            _projectileDetails.TravelTime = StateData.travelTime;
            
            // 速度曲線
            _projectileDetails.SpeedCurve = StateData.speedCurve;
            
            // 翻轉 ?
            _projectileDetails.ShouldFlip = Movement.FacingDirection == -1;

            // == Combat ==
            _projectileDetails.Damage = StateData.damage;
            _projectileDetails.PoiseDamage = StateData.poiseDamage;
            _projectileDetails.KnockbackStrength = StateData.knockbackStrength;
            _projectileDetails.KnockbackDirection = StateData.knockbackDirection;

            // 使用重力 ?
            _projectileDetails.UseFallGravity = StateData.useFallGravity;
            
            // 使用者位置
            _projectileDetails.ProjectileUser = Enemy.transform;

            // 可互動圖層
            _projectileDetails.InteractableLayers = StateData.detectableLayers;


            // == Projectile Settings ==
            // var projectile = Enemy.ObjectPool.GetObject(StateData.projectilePrefab);
            // var projectileScript = projectile.GetComponent<IProjectile>();
            //
            // if (projectileScript != null)
            // {
            //     StartProjectile(projectileScript);
            // }
            // else
            // {
            //     Debug.LogError(" Projectile does not have the right script attached ");
            // }
        }

        private void StartProjectile(IProjectile script)
        {
            script.SetProjectileStartingState(_projectileDetails);
        }
        
        #endregion
        
        #region w/ Attack

        public override void TriggerAttack()
        {
            base.TriggerAttack();
            
            InitializeProjectileData();
            
            // == Projectile Settings ==
            var projectile = ObjectPoolManager.Instance.GetObject(StateData.projectilePrefab);
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
        

    }
}