using UnityEngine;

using ChittaExorcist.CharacterCore;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public abstract class ShaoYueAbilityState<T> : ShaoYuePlayerState<T> where T : PlayerStateData
    {
        protected ShaoYueAbilityState(string animationBoolName, ShaoYuePlayer player) : base(animationBoolName, player)
        {
            // 1. Idle
            // 2. InAir
        }
        
        #region w/ Core Components

        // Movement
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);
        private Movement _movement;

        // CollisionSenses
        protected CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses : Core.GetCoreComponent(out _collisionSenses);
        private CollisionSenses _collisionSenses;

        // Player Mana
        protected PlayerManaStats PlayerManaStats => _playerManaStats ? _playerManaStats : Core.GetCoreComponent(out _playerManaStats);
        private PlayerManaStats _playerManaStats;
        
        #endregion

        #region w/ Variables

        // Input
        protected int XInput;
        protected int YInput;
        
        protected bool JumpInput;
        // protected bool DashInput;
        protected bool AttackInput;

        // Check
        protected bool IsGrounded;
        protected bool IsTouchingWall;

        // Ability Check
        protected bool IsAbilityDone;
        protected bool IsAbilityEarlyOut;

        #endregion

        #region w/ State Workflow

        public override void DoCheck()
        {
            base.DoCheck();
            if (!CollisionSenses)
            {
                Debug.LogWarning(" 無法取得 CollisionSenses ");
                return;
            }
            IsGrounded = CollisionSenses.Ground;
            IsTouchingWall = CollisionSenses.WallFront;
        }

        public override void DoInput()
        {
            base.DoInput();

            XInput = InputHandler.NormalizedXInput;
            YInput = InputHandler.NormalizedYInput;
            JumpInput = InputHandler.JumpInput;
            // DashInput = InputHandler.DashInput;
            AttackInput = InputHandler.AttackInput;
        }

        public override void Enter()
        {
            base.Enter();
            IsAbilityDone = false;
            IsAbilityEarlyOut = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // 動作結束才須判斷 state 轉換
            if (!IsAbilityDone) return;

            if (!Movement)
            {
                Debug.LogWarning(" 無法取得 Movement ");
                return;
            }

            if (PlayerManaStats.ManaIsZero())
            {
                StateMachine.ChangePlayerAndState(Player, Player.ChuXiao, Player.IdleState, Player.ChuXiao.IdleState);
                return;
            }
            
            if (IsGrounded && Movement.CurrentVelocity.y < 0.01f && !Player.InAirState.IsJumping)
            {
                // Idle
                StateMachine.ChangeState(Player.IdleState);
            }
            else
            {
                // InAir
                StateMachine.ChangeState(Player.InAirState);
            }
        }

        #endregion
    }
}