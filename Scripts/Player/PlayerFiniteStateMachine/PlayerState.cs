using UnityEngine;

using ChittaExorcist.CharacterCore;
using ChittaExorcist.PlayerSettings.InputHandler;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public abstract class PlayerState
    {
        #region w/ Components

        protected Core Core { get; set; }
        // TODO: 再考慮看看是否會使用到
        // protected PlayerData Data { get; set; }
        protected PlayerStateMachine StateMachine { get; set; }
        protected PlayerInputHandler InputHandler { get; set; }
    
        #endregion

        #region w/ Variables

        protected bool IsAnimationFinished;
        // TODO: state 中斷設置要在哪?
        protected bool IsAnimationMinimumRequirementMet;
        protected bool IsExitingState;
        protected float StartTime;
        protected readonly string AnimationBoolName;

        protected float StateDuration => Time.time - StartTime;

        #endregion

        #region w/ Constructor

        protected PlayerState(string animationBoolName)
        {
            AnimationBoolName = animationBoolName;
        }

        #endregion

        #region w/ State Workflow

        public virtual void DoCheck() { }
    
        public virtual void DoInput() { }
    
        public virtual void Enter()
        {
            DoCheck();
            StartTime = Time.time;
            IsAnimationFinished = false;
            IsAnimationMinimumRequirementMet = false;
            IsExitingState = false;
        }

        public virtual void Exit()
        {
            IsExitingState = true;
        }

        public virtual void LogicUpdate()
        {
            DoInput();
        }

        public virtual void PhysicsUpdate()
        {
            DoCheck();
        }
    
        #endregion

        #region w/ Animation Trigger Functions

        public virtual void AnimationTrigger() { }
        public virtual void AnimationFinishTrigger() => IsAnimationFinished = true;
        public virtual void AnimationMinimumRequirementMeetTrigger() => IsAnimationMinimumRequirementMet = true;

        #endregion
    }

    /// <summary>
    /// FSM 的 PlayerState
    /// </summary>
    /// <typeparam name="T1">Player State Data</typeparam>
    /// <typeparam name="T2">Player</typeparam>
    public abstract class PlayerState<T1, T2> : PlayerState where T1 : PlayerStateData where T2 : Player
    {
        public T1 StateData { get; protected set; }
        public T2 Player { get; protected set; }

        protected PlayerState(string animationBoolName, T2 player) : base(animationBoolName)
        {
            Player = player;
            Core = player.Core;
            StateMachine = player.StateMachine;
            InputHandler = player.InputHandler;
            // 注意 Data 取得先後
            StateData = player.Data.GetData<T1>();
        }

        public override void Enter()
        {
            base.Enter();
            Player.Animator.SetBool(AnimationBoolName, true);
        }
        
        public override void Exit()
        {
            base.Exit();
            Player.Animator.SetBool(AnimationBoolName, false);
        }
    }
}