using UnityEngine;

using ChittaExorcist.CharacterCore;

namespace ChittaExorcist.EnemySettings.FSM
{
    public abstract class E_PlayerDetectedState<T1> : EnemyState<T1, ED_PlayerDetectedStateSO> where T1 : Enemy
    {
        protected E_PlayerDetectedState(string animationBoolName, T1 enemy, ED_PlayerDetectedStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // DoCheck      => 判定玩家是否在 min aggro and max aggro and close range, wall front, ledge vertical, wall back
            // Enter        => 設定 x 速度為 0, ShouldPerformLongRangeAction = false
            // Exit         => n
            // LogicUpdate  => 設定 x 速度為 0, 確認 long range action time 是否結束, ShouldPerformLongRangeAction = true
        }
        
        #region w/ Core Components
    
        // Movement
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);
        private Movement _movement;
        
        // CollisionSenses
        protected CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses : Core.GetCoreComponent(out _collisionSenses);
        private CollisionSenses _collisionSenses;
        
        #endregion

        #region w/ Player Detected

        protected bool IsPlayerInMinAggroRange;         // 玩家在最近仇恨範圍
        protected bool IsPlayerInMaxAggroRange;         // 玩家在最遠仇恨範圍
        protected bool ShouldPerformLongRangeAction;    // 玩家在最遠反映範圍
        protected bool ShouldPerformCloseRangeAction;   // 玩家在最近反映範圍
        protected bool IsDetectingLedge; // 偵測牆壁
        protected bool IsDetectingWall;  // 偵測邊界
        protected bool IsLedgeVerticalBack;

        protected bool IsWallBack;

        #endregion
    
        #region w/ State Workflow

        protected override void DoCheck()
        {
            base.DoCheck();
            if (!CollisionSenses)
            {
                Debug.LogWarning("Enemy PlayerDetected State 無法取得 CollisionSenses");
                return;
            }

            IsDetectingLedge = CollisionSenses.LedgeVertical;
            IsDetectingWall = CollisionSenses.WallFront;
            IsWallBack = CollisionSenses.WallBack;
            IsLedgeVerticalBack = CollisionSenses.LedgeVerticalBack;
            
            IsPlayerInMinAggroRange = Enemy.CheckPlayerInMinAggroRange();
            IsPlayerInMaxAggroRange = Enemy.CheckPlayerInMaxAggroRange();
            ShouldPerformCloseRangeAction = Enemy.CheckPlayerInCloseRangeAction();
        }

        public override void Enter()
        {
            base.Enter();
            if (!Movement)
            {
                Debug.LogWarning("Enemy PlayerDetected State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityX(0f);
            
            ShouldPerformLongRangeAction = false;
        }
    
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!Movement)
            {
                Debug.LogWarning("Enemy PlayerDetected State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityX(0f);

            if (Time.time >= StartTime + StateData.longRangeActionTime)
            {
                ShouldPerformLongRangeAction = true;
            }
        }

        #endregion
        
    }
}