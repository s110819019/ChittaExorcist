using ChittaExorcist.CharacterCore;
using UnityEngine;

namespace ChittaExorcist.NPCSettings.FSM
{
    public class N_ReactState<T1> : NPCState<T1, ND_ReactStateSO> where T1 : NPC
    {
        public N_ReactState(string animationBoolName, T1 npc, ND_ReactStateSO stateData) : base(animationBoolName, npc, stateData)
        {
            EventHandler.OnFinishReact += FinishReact;
        }
        
        #region w/ Core Components
    
        // Movement
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);
        private Movement _movement;

        #endregion

        #region w/ React

        protected bool IsAnimationFinished;

        protected virtual void FinishReact()
        {
            IsAnimationFinished = true;
        }
    
        #endregion
    
        #region w/ State Workflow
    
        public override void Enter()
        {
            base.Enter();
        
            // NPC.AnimationToStateMachine.ReactState = this;
            
            IsAnimationFinished = false;
        
            if (!Movement)
            {
                Debug.LogWarning("NPC React State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityX(0f);

            // IsIdleTimeOver = false;
            // SetRandomIdleTime(); // 隨機等待時間
        }

        public override void Exit()
        {
            base.Exit();
        
            // if (!ShouldFlipAfterIdle) return;
            //
            // if (Movement) Movement.Flip();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        
            if (!Movement)
            {
                Debug.LogWarning("NPC React State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityX(0f);

            // if (Time.time >= StartTime + IdleTime)
            // {
            //     IsIdleTimeOver = true; // 時間到則切換狀態
            // }
        }

        #endregion
    }
}