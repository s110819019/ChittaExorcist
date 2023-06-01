using ChittaExorcist.CharacterCore;
using UnityEngine;

namespace ChittaExorcist.NPCSettings.FSM
{
    public abstract class NPCState
    {
        #region w/ Components
        
        protected Core Core { get; set; }
        protected NPCStateMachine StateMachine { get; set; }
        protected NPCAnimationEventHandler EventHandler { get; set; }

        #endregion

        #region w/ Variables

        public float StartTime { get; private set; } // state 開始時間

        protected readonly string AnimationBoolName;

        #endregion

        #region w/ Constructor

        protected NPCState(string animationBoolName)
        {
            AnimationBoolName = animationBoolName;
        }    

        #endregion

        #region w/ State Workflow

        protected virtual void DoChecks() { }
    
        public virtual void Enter()
        {
            DoChecks();
            StartTime = Time.time;
        }

        public virtual void Exit() { }

        public virtual void LogicUpdate() { }

        public virtual void PhysicsUpdate()
        {
            DoChecks();
        }    

        #endregion
    }
    
    public abstract class NPCState<T1, T2> : NPCState where T1 : NPC where T2 : NPCStateDataSO
    {
        public T1 NPC { get; protected set; }
        public T2 StateData { get; protected set; }
        
        protected NPCState(string animationBoolName, T1 npc, T2 stateData) : base(animationBoolName)
        {
            NPC = npc;
            Core = npc.Core;
            StateMachine = npc.StateMachine;
            EventHandler = npc.EventHandler;
            StateData = stateData;
        }

        public override void Enter()
        {
            base.Enter();
            NPC.Animator.SetBool(AnimationBoolName, true);
        }

        public override void Exit()
        {
            base.Exit();
            NPC.Animator.SetBool(AnimationBoolName, false);
        }
    }
}