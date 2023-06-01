using UnityEngine;

using ChittaExorcist.CharacterCore;

namespace ChittaExorcist.EnemySettings.FSM
{
    public abstract class E_ChargeState<T1> : EnemyState<T1, ED_ChargeStateSO> where T1 : Enemy
    {
        protected E_ChargeState(string animationBoolName, T1 enemy, ED_ChargeStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // DoCheck      => 判定玩家是否在 min aggro and close range, wall front, ledge vertical
            // Enter        => IsChargeTimeOver = false
            // Exit         => n
            // LogicUpdate  => 設定 x 速度為 charge speed, 確認 charge time 是否結束, IsChargeTimeOver = true
        }
        
        #region w/ Core Components
    
        // Movement
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);
        private Movement _movement;
        
        // CollisionSenses
        protected CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses : Core.GetCoreComponent(out _collisionSenses);
        private CollisionSenses _collisionSenses;    

        #endregion

        #region w/ Collision Check

        protected bool IsDetectingWall;
        protected bool IsDetectingLedge;
    
        protected bool IsChargeTimeOver; // 衝刺時間結束
    
        protected bool IsPlayerInMinAggroRange;
        protected bool ShouldPerformCloseRangeAction;

        #endregion
    
        #region w/ State Workflow

        protected override void DoCheck()
        {
            base.DoCheck();
            if (!CollisionSenses)
            {
                Debug.LogWarning("Enemy Charge State 無法取得 CollisionSenses");
                return;
            }
            
            
            IsDetectingWall = CollisionSenses.WallFront;
            IsDetectingLedge = CollisionSenses.LedgeVertical;   
            
            IsPlayerInMinAggroRange = Enemy.CheckPlayerInMinAggroRange();          // 玩家在最小攻擊範圍
            ShouldPerformCloseRangeAction = Enemy.CheckPlayerInCloseRangeAction(); // 玩家在接近範圍
        }
    
        public override void Enter()
        {
            base.Enter();
            IsChargeTimeOver = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!Movement)
            {
                Debug.LogWarning("Enemy Charge State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityX(StateData.chargeSpeed * Movement.FacingDirection);
        
            if (Time.time >= StartTime + StateData.chargeTime)
            {
                // 衝刺時間結束
                IsChargeTimeOver = true;
            }
        }    

        #endregion
    }
}