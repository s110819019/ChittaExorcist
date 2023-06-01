using UnityEngine;

using ChittaExorcist.CharacterCore;

namespace ChittaExorcist.EnemySettings.FSM
{
    public abstract class E_HitState<T1> : EnemyState<T1, ED_HitStateSO> where T1 : Enemy
    {
        protected E_HitState(string animationBoolName, T1 enemy, ED_HitStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            EventHandler.OnFinishAnim += FinishHit;
            // DoCheck      => n
            // Enter        => IsBeatenTimeOver = false
            // Exit         => n
            // LogicUpdate  => 設定 x 速度為 0, 確認 beaten time 是否結束, IsBeatenTimeOver = true, 紀錄 LastGetBeatenTime
        }
        
        #region w/ Core Components
    
        // Movement
        private Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);
        private Movement _movement;

        #endregion

        #region w/ Hit

        protected float LastGetBeatenTime;
        protected bool IsBeatenTimeOver;

        protected bool IsAnimationFinished;

        public virtual void FinishHit() // 動畫結束
        {
            IsAnimationFinished = true;
        }
        
        #endregion
    
        #region w/ State Workflow

        public override void Enter()
        {
            base.Enter();
            IsAnimationFinished = false;
            IsBeatenTimeOver = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!Movement)
            {
                Debug.LogWarning("Enemy Hit State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityX(0f);
        
            if (Time.time >= StartTime + StateData.getBeatenTime)
            {
                IsBeatenTimeOver = true;
            }
            
            LastGetBeatenTime = Time.time;
        }

        #endregion
    }
}