using ChittaExorcist.CharacterCore;
using UnityEngine;

namespace ChittaExorcist.NPCSettings.FSM
{
    public class N_IdleState<T1> : NPCState<T1, ND_IdleStateSO> where T1 : NPC
    {
        public N_IdleState(string animationBoolName, T1 npc, ND_IdleStateSO stateData) : base(animationBoolName, npc, stateData)
        {
        }
        
        #region w/ Core Components
    
        // Movement
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);
        private Movement _movement;

        #endregion
    
        #region w/ Idle

        protected bool ShouldFlipAfterIdle;
        protected bool IsIdleTimeOver;
        protected float IdleTime;
        public void SetFlipAfterIdle(bool value) // 用在 move state 判斷走到邊緣 須返回走
        {
            ShouldFlipAfterIdle = value;
        }
        private void SetRandomIdleTime() // 設定隨機待機時間
        {
            IdleTime = Random.Range(StateData.minIdleTime, StateData.maxIdleTime);
        }
    
        #endregion
    
        #region w/ State Workflow
    
        public override void Enter()
        {
            base.Enter();
            
            if (!Movement)
            {
                Debug.LogWarning("NPC Idle State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityX(0f);
            IsIdleTimeOver = false;
            SetRandomIdleTime(); // 隨機等待時間
        }

        public override void Exit()
        {
            base.Exit();
            if (!ShouldFlipAfterIdle) return;
        
            if (!Movement)
            {
                Debug.LogWarning("NPC Idle State 無法取得 Movement");
                return;
            }
            
            Movement.Flip();
            
            ShouldFlipAfterIdle = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        
            if (!Movement)
            {
                Debug.LogWarning("NPC Idle State 無法取得 Movement");
                return;
            }

            Movement.SetVelocityX(0f);

            if (Time.time >= StartTime + IdleTime)
            {
                IsIdleTimeOver = true; // 時間到則切換狀態
            }
        }

        #endregion
    }
}