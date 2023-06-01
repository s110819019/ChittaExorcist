using UnityEngine;

using ChittaExorcist.CharacterCore;

namespace ChittaExorcist.EnemySettings.FSM
{
    public abstract class E_AttackState<T1, T2> : EnemyState<T1, T2> where T1 : Enemy where T2 : EnemyStateDataSo
    {
        // attack transform
        protected readonly Transform AttackPosition;
        
        protected E_AttackState(string animationBoolName, T1 enemy, T2 stateData, Transform attackPosition) : base(animationBoolName, enemy, stateData)
        {
            EventHandler.OnFinish += FinishAttack;
            AttackPosition = attackPosition;
            // DoCheck      => 判定玩家是否在 min aggro
            // Enter        => 設定 x 速度為 0, IsAnimationFinished = false
            // Exit         => n
            // LogicUpdate  => 設定 x 速度為 0
        }
        
        #region w/ Core Components
    
        // Movement
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);
        private Movement _movement;    

        #endregion
    
        #region w/ Attack

        protected bool IsAnimationFinished;
        protected bool IsPlayerInMinAggroRange;
    
        // AnimationToStateMachine 會在動畫事件中使用到
        public virtual void TriggerAttack() { }     // 觸發攻擊
        public virtual void FinishAttack()          // 動畫結束
        {
            IsAnimationFinished = true;
        }
        public virtual void TriggerAttackEffect() { } // 攻擊特效

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
                Debug.LogWarning("Enemy Attack State 無法取得 Movement");
                return;
            }

            Movement.SetVelocityX(0f);

            // TODO: Animation To State Machine
            // Enemy.AnimationToStateMachine.AttackState = this;
            
            
            IsAnimationFinished = false;
        }
    
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!Movement)
            {
                Debug.LogWarning("Enemy Attack State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityX(0f);
        }

        #endregion
    }
}