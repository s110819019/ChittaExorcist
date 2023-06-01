using UnityEngine;
using ChittaExorcist.CharacterCore;
using ChittaExorcist.GameCore;

namespace ChittaExorcist.EnemySettings.FSM
{
    public abstract class E_DeadState<T1> : EnemyState<T1, ED_DeadStateSO> where T1 : Enemy
    {
        protected E_DeadState(string animationBoolName, T1 enemy, ED_DeadStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // DoCheck      => n
            // Enter        => 設定 x 速度為 0
            // Exit         => n
            // LogicUpdate  => 設定 x 速度為 0
        }
        
        #region w/ Core Components
    
        // Movement
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);
        private Movement _movement;
        
        // CollisionSenses
        protected CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses : Core.GetCoreComponent(out _collisionSenses);
        private CollisionSenses _collisionSenses;
        
        protected DamageReceiver DamageReceiver => _damageReceiver ? _damageReceiver : Core.GetCoreComponent(out _damageReceiver);
        private DamageReceiver _damageReceiver;
        
        #endregion

        private Collider2D _collider;
        
        #region w/ State Workflow

        public override void Enter()
        {
            base.Enter();
            if (!Movement)
            {
                Debug.LogWarning("Enemy Dead State 無法取得 Movement");
                return;
            }

            if (Enemy.Data.dropSoul && Enemy.Data.soul != null)
            {
                var soulObject = ObjectPoolManager.Instance.GetObject(Enemy.Data.soul);
                soulObject.transform.position = Enemy.transform.position;
            }
            
            Movement.SetVelocityX(0f);

            if (DamageReceiver.TryGetComponent(out _collider))
            {
                _collider.enabled = false;
            }
            
            // if (Entity.BodyDamage != null)
            // {
            //     // TODO: 不太好
            //     Entity.BodyDamage.gameObject.SetActive(false);
            // }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!Movement)
            {
                Debug.LogWarning("Enemy Dead State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityX(0f);
        }

        #endregion
    }
}