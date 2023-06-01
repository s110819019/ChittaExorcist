using ChittaExorcist.CharacterCore;
using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public abstract class E_DodgeState<T1> : EnemyState<T1, ED_DodgeStateSO> where T1 : Enemy
    {
        public E_DodgeState(string animationBoolName, T1 enemy, ED_DodgeStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
        }

        #region w/ Dodge

        protected bool ShouldPerformCloseRangeAction;
        protected bool IsPlayerInMaxAggroRange;
        protected bool IsGrounded;
        protected bool IsWallBack;
        protected bool IsLedgeVerticalBack;
    
        protected bool IsDodgeOver;

        protected float LastDodgeTime;

        public bool CanDodge => Time.time >= LastDodgeTime + StateData.dodgeCooldown;

        #endregion
        
        #region w/ Core Components
    
        // Movement
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);
        private Movement _movement;
        // CollisionSenses
        protected CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses : Core.GetCoreComponent(out _collisionSenses);
        private CollisionSenses _collisionSenses;    

        #endregion
        
        #region w/ State Workflow

        protected override void DoCheck()
        {
            base.DoCheck();

            ShouldPerformCloseRangeAction = Enemy.CheckPlayerInCloseRangeAction();
            IsPlayerInMaxAggroRange = Enemy.CheckPlayerInMaxAggroRange();
            
            if (!CollisionSenses)
            {
                Debug.LogWarning("Enemy Dodge State 無法取得 CollisionSenses");
                return;
            }
            
            IsGrounded = CollisionSenses.Ground;
            IsWallBack = CollisionSenses.WallBack;
            IsLedgeVerticalBack = CollisionSenses.LedgeVerticalBack;
        }

        public override void Enter()
        {
            base.Enter();
        
            IsDodgeOver = false;
            
            if (!Movement)
            {
                Debug.LogWarning("Enemy Move State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityZero();
        }

        public override void Exit()
        {
            base.Exit();
            LastDodgeTime = Time.time;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // if (Time.time >= StartTime + StateData.dodgeTime && IsGrounded)

            if (!Movement)
            {
                Debug.LogWarning("Enemy Move State 無法取得 Movement");
                return;
            }
            
            if (Time.time >= StartTime + StateData.dodgeTime)
            {
                IsDodgeOver = true;
            }
            else
            {
                Movement.SetVelocity(StateData.speedCurve.Evaluate(Duration), StateData.dodgeAngle,
                    -Movement.FacingDirection);
            }
        }    

        #endregion
    }
}