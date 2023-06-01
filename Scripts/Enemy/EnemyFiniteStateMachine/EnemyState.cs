using UnityEngine;

using ChittaExorcist.CharacterCore;

namespace ChittaExorcist.EnemySettings.FSM
{
    public abstract class EnemyState
    {
        #region w/ Components

        protected Core Core { get; set; }
        protected EnemyStateMachine StateMachine { get; set; }
        // TODO: Not sure animation work
        protected EnemyAnimationEventHandler EventHandler { get; set; }

        #endregion

        #region w/ Variables

        protected float StartTime;
        protected float Duration => Time.time - StartTime;
        protected readonly string AnimationBoolName;

        #endregion

        #region w/ Constructor

        protected EnemyState(string animationBoolName)
        {
            AnimationBoolName = animationBoolName;
        }

        #endregion

        #region w/ State Workflow

        protected virtual void DoCheck() { }
        
        public virtual void Enter()
        {
            DoCheck();
            StartTime = Time.time;
        }

        public virtual void Exit() { }

        public virtual void LogicUpdate() { }

        public virtual void PhysicsUpdate()
        {
            DoCheck();
        }
        
        #endregion
    }

    public abstract class EnemyState<T1, T2> : EnemyState where T1 : Enemy where T2 : EnemyStateDataSo
    {
        public T1 Enemy { get; protected set; }
        public T2 StateData { get; protected set; }
        
        protected EnemyState(string animationBoolName, T1 enemy, T2 stateData) : base(animationBoolName)
        {
            Enemy = enemy;
            Core = enemy.Core;
            StateMachine = enemy.StateMachine;
            EventHandler = enemy.EventHandler;
            
            StateData = stateData;
        }

        public override void Enter()
        {
            base.Enter();
            Enemy.Animator.SetBool(AnimationBoolName, true);
        }

        public override void Exit()
        {
            base.Exit();
            Enemy.Animator.SetBool(AnimationBoolName, false);
        }
    }
}
