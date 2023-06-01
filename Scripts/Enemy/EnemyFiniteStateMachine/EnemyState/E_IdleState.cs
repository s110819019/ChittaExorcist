using UnityEngine;

using ChittaExorcist.CharacterCore;

namespace ChittaExorcist.EnemySettings.FSM
{
    public abstract class E_IdleState<T1> : EnemyState<T1, ED_IdleStateSO> where T1 : Enemy
    {
        protected E_IdleState(string animationBoolName, T1 enemy, ED_IdleStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // DoCheck      => 判定玩家是否在 min aggro
            // Enter        => 設定 x 速度為 0, 設定隨機 idle 時間, IsIdleTimeOver = false
            // Exit         => 確認結束 idle state 是否需要要翻轉(若在 move state 中判斷走到邊緣), 則翻轉
            // LogicUpdate  => 設定 x 速度為 0, 確認 idle time 是否結束, IsIdleTimeOver = true
        }

        #region w/ Core Components
    
        // Movement
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);
        private Movement _movement;
        
        // CollisionSenses
        protected CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses : Core.GetCoreComponent(out _collisionSenses);
        private CollisionSenses _collisionSenses;    

        #endregion
        
        #region w/ Idle

        protected bool ShouldFlipAfterIdle;
        protected bool IsIdleTimeOver;
        protected float IdleTime;
        
        // 用在 move state 判斷走到邊緣 須返回走
        public void SetFlipAfterIdle(bool value)
        {
            ShouldFlipAfterIdle = value;
        }
        
        private void SetRandomIdleTime() // 設定隨機待機時間
        {
            IdleTime = Random.Range(StateData.minIdleTime, StateData.maxIdleTime);
        }
    
        #endregion

        #region w/ Collision Check

        protected bool IsPlayerInMinAggroRange;

        #endregion
    
        #region w/ State Workflow

        protected override void DoCheck()
        {
            base.DoCheck();
            IsPlayerInMinAggroRange = Enemy.CheckPlayerInMinAggroRange();
        }

        public override void Enter()
        {
            base.Enter();
            if (!Movement)
            {
                Debug.LogWarning("Enemy Idle State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityX(0f);
            
            IsIdleTimeOver = false;
            
            // 隨機等待時間
            SetRandomIdleTime();
        }

        public override void Exit()
        {
            base.Exit();
            if (!Movement)
            {
                Debug.LogWarning("Enemy Idle State 無法取得 Movement");
                return;
            }
            
            if (!ShouldFlipAfterIdle) return;

            if (IsPlayerInMinAggroRange)
            {
                return;
            }
            
            // Debug.Log("Flip");
            Movement.Flip();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!Movement)
            {
                Debug.LogWarning("Enemy Idle State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityX(0f);

            if (Time.time >= StartTime + IdleTime)
            {
                // 時間到則可切換狀態
                IsIdleTimeOver = true;
            }
        }

        #endregion
    }
}
