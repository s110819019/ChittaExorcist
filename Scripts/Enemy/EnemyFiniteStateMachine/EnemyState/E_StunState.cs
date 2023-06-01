using UnityEngine;

using ChittaExorcist.CharacterCore;

namespace ChittaExorcist.EnemySettings.FSM
{
    public abstract class E_StunState<T1> : EnemyState<T1, ED_StunStateSO> where T1 : Enemy
    {
        protected E_StunState(string animationBoolName, T1 enemy, ED_StunStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // DoCheck      => 判定玩家是否在 min aggro and close range
            // Enter        => 設定 x 速度為 knock back speed, IsStunTimeOver = false, IsMovementStopped = false
            // Exit         => n?
            // LogicUpdate  => 設定 x 速度為 knock back speed, 落地且 knock back 時間已到則設定 x 速度為 0,  IsMovementStopped = true, 確認 stun time 是否結束, IsStunTimeOver = true
        }

        #region w/ Core Components
    
        // Movement
        private Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);
        private Movement _movement;
        
        // CollisionSenses
        private CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses : Core.GetCoreComponent(out _collisionSenses);
        private CollisionSenses _collisionSenses;    

        #endregion

        #region w/ Stun

        protected bool IsStunTimeOver;
        protected bool IsGrounded;
        protected bool IsMovementStopped;
        protected bool PerformCloseRangeAction;
        protected bool IsPlayerInMinAggroRange;

        #endregion

        #region w/ State Workflow

        protected override void DoCheck()
        {
            base.DoCheck();
            if (!CollisionSenses)
            {
                Debug.LogWarning("Enemy Stun State 無法取得 CollisionSenses");
                return;
            }
            
            IsGrounded = CollisionSenses.Ground;
            PerformCloseRangeAction = Enemy.CheckPlayerInCloseRangeAction();
            IsPlayerInMinAggroRange = Enemy.CheckPlayerInMinAggroRange();
        }

        public override void Enter()
        {
            base.Enter();
            if (!Movement)
            {
                Debug.LogWarning("Enemy Stun State 無法取得 Movement");
                return;
            }
            
            IsStunTimeOver = false;
            IsMovementStopped = false;
            
            // Movement.SetVelocity(StateData.knockBackSpeed, StateData.knockBackAngle, Enemy.LastDamageDirection);
        }

        public override void Exit()
        {
            base.Exit();
            // Entity.ResetStunResistance();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!Movement)
            {
                Debug.LogWarning("Enemy Stun State 無法取得 Movement");
                return;
            }
            
            // Movement.SetVelocity(StateData.knockBackSpeed, StateData.knockBackAngle, Enemy.LastDamageDirection);

            if (Time.time >= StartTime + StateData.stunTime)
            {
                IsStunTimeOver = true;
            }

            if (IsGrounded && Time.time >= StartTime + StateData.stunKnockBackTime && !IsMovementStopped)
            {
                IsMovementStopped = true;
                Movement.SetVelocityX(0f);
            }
        }

        #endregion
    }
}