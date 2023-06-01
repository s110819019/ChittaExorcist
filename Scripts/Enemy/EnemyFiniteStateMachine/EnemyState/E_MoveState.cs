using ChittaExorcist.CharacterCore;
using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public abstract class E_MoveState<T1> : EnemyState<T1, ED_MoveStateSO> where T1 : Enemy
    {
        protected E_MoveState(string animationBoolName, T1 enemy, ED_MoveStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // DoCheck      => 判定玩家是否在 min aggro, wall front, ledge vertical
            // Enter        => 設定 x 速度為 move speed
            // Exit         => n
            // LogicUpdate  => 設定 x 速度為 move speed
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
        protected bool IsPlayerInMinAggroRange;

        #endregion

        #region w/ State Workflow

        protected override void DoCheck()
        {
            base.DoCheck();
        
            IsPlayerInMinAggroRange = Enemy.CheckPlayerInMinAggroRange();

            if (!CollisionSenses)
            {
                Debug.LogWarning("Enemy Move State 無法取得 CollisionSenses");
                return;
            }
            
            IsDetectingLedge = CollisionSenses.LedgeVertical;
            IsDetectingWall = CollisionSenses.WallFront;
        }
    
        public override void Enter()
        {
            base.Enter();
            if (!Movement)
            {
                Debug.LogWarning("Enemy Move State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityX(StateData.movementSpeed * Movement.FacingDirection);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!Movement)
            {
                Debug.LogWarning("Enemy Idle State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityX(StateData.movementSpeed * Movement.FacingDirection);
        }    

        #endregion
    }
}