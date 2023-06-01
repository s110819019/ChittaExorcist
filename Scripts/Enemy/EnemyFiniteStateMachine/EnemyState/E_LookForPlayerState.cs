using UnityEngine;

using ChittaExorcist.CharacterCore;

namespace ChittaExorcist.EnemySettings.FSM
{
    public abstract class E_LookForPlayerState<T1> : EnemyState<T1, ED_LookForPlayerStateSO> where T1 : Enemy
    {
        protected E_LookForPlayerState(string animationBoolName, T1 enemy, ED_LookForPlayerStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // DoCheck      => 判定玩家是否在 min aggro
            // Enter        => 設定 x 速度為 0, 設定轉身相關初始變數(turn done, turn time over, last turn time, amount of turn done)
            // Exit         => n
            // LogicUpdate  => 設定 x 速度為 0, turn 相關確認
        }
        
        #region w/ Core Components
    
        // Movement
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);
        private Movement _movement;

        #endregion

        #region w/ Look for Player

        protected bool IsPlayerInMinAggroRange;
    
        protected bool ShouldTurnImmediately;
        protected bool IsAllTurnsDone;
        protected bool IsAllTurnsTimeOver;

        protected float LastTurnTime;

        protected int AmountOfTurnsDone;
    
        public void SetTurnImmediately(bool value) // 是否該立即轉身
        {
            ShouldTurnImmediately = value;
        }

        #endregion
    
        #region w/ State Workflow

        protected override void DoCheck()
        {
            base.DoCheck();
            IsPlayerInMinAggroRange = Enemy.CheckPlayerInMinAggroRange(); // 玩家是否在接近範圍
        }
    
        public override void Enter()
        {
            base.Enter();
            if (!Movement)
            {
                Debug.LogWarning("Enemy LookForPlayer State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityX(0f);

            IsAllTurnsDone = false;
            IsAllTurnsTimeOver = false;
            LastTurnTime = StartTime;
            AmountOfTurnsDone = 0;
        }
    
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!Movement)
            {
                Debug.LogWarning("Enemy LookForPlayer State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityX(0f);
            
            // 立即轉身
            if (ShouldTurnImmediately)
            {
                Movement.Flip();
                
                LastTurnTime = Time.time;
                
                AmountOfTurnsDone++;
                
                ShouldTurnImmediately = false;
            }
            // 正常轉身
            else if (Time.time >= LastTurnTime + StateData.timeBetweenTurns && !IsAllTurnsDone)
            {
                Movement.Flip();
                
                LastTurnTime = Time.time;
                
                AmountOfTurnsDone++;
            }
            
            // 確認已轉身至設定次數
            if (AmountOfTurnsDone >= StateData.amountOfTurns)
            {
                IsAllTurnsDone = true;
            }
            
            // 已經轉身至設定次數
            if (Time.time >= LastTurnTime + StateData.timeBetweenTurns && IsAllTurnsDone)
            {
                IsAllTurnsTimeOver = true;
            }
        }

        #endregion
    }
}