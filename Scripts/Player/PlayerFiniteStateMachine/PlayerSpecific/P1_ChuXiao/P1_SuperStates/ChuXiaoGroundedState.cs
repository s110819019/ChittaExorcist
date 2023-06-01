using UnityEngine;

using ChittaExorcist.CharacterCore;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public abstract class ChuXiaoGroundedState<T> : ChuXiaoPlayerState<T> where T : PlayerStateData
    {
        protected ChuXiaoGroundedState(string animationBoolName, ChuXiaoPlayer player) : base(animationBoolName, player)
        {
            // Light Attack ?
            
            // 1. Dash
            // 2. Jump
            // 3. InAir
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
        
        // Player Health
        protected PlayerHealthStats PlayerHealthStats => _playerHealthStats ? _playerHealthStats : Core.GetCoreComponent(out _playerHealthStats);
        private PlayerHealthStats _playerHealthStats;
        
        #endregion

        #region w/ Variables

        // Input
        protected int XInput;
        protected int YInput;
        
        protected bool JumpInput;
        protected bool DashInput;
        protected bool AttackInput;

        protected bool ChargeInput;

        // TODO: How Switch
        protected bool SwitchInput;

        protected bool BlockInput;

        // Check
        protected bool IsGrounded;
        protected bool IsWallFront;

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
            IsWallFront = CollisionSenses.WallFront;
        }
        
        public override void DoInput()
        {
            base.DoInput();
            
            XInput = InputHandler.NormalizedXInput;
            YInput = InputHandler.NormalizedYInput;
            JumpInput = InputHandler.JumpInput;
            DashInput = InputHandler.DashInput;
            AttackInput = InputHandler.AttackInput;

            SwitchInput = InputHandler.SwitchInput;

            ChargeInput = InputHandler.ChargeInput;

            BlockInput = InputHandler.BlockInput;
        }
        
        public override void Enter()
        {
            base.Enter();
            
            // 重設跳躍次數
            Player.JumpState.ResetAmountOfJumpsLeft();
            
            // 重設衝刺
            Player.DashState.ResetCanDash();
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!Movement)
            {
                Debug.LogWarning(" 無法取得 Movement ");
                return;
            }

            // if (AttackInput && PlayerManaStats.ManaIsFull())
            // {
            //     InputHandler.UseAttackInput();
            //     Player.ShaoYue.LightAttackState.Ability.SetIsFirstAttack();
            //     StateMachine.ChangePlayerAndState(Player, Player.ShaoYue, Player.IdleState, Player.ShaoYue.LightAttackState);
            // }
            // else if (DashInput && Player.DashState.CanDash && PlayerManaStats.ManaIsFull())
            // {
            //     // Dash
            //     Player.DashState.SetShouldSwitchToShaoYue();
            //     StateMachine.ChangeState(Player.DashState);
            // }
            // else if (JumpInput && Player.JumpState.CanJump && PlayerManaStats.ManaIsFull())
            // {
            //     // Jump(Shao Yue)
            //     StateMachine.ChangePlayerAndState(Player, Player.ShaoYue, Player.IdleState, Player.ShaoYue.JumpState);
            // }
            // else if (BlockInput && PlayerManaStats.ManaIsFull())
            // {
            //     // Block(Shao Yue)
            //     // Player.ShaoYue.InputHandler.SetBlockInput();
            //     StateMachine.ChangePlayerAndState(Player, Player.ShaoYue, Player.IdleState, Player.ShaoYue.BlockState);
            // }
            if (SwitchInput && PlayerManaStats.ManaIsFull() && Player.PlayerHolder.CanSwitchCharacter)
            {
                InputHandler.UseSwitchInput();
                StateMachine.ChangePlayerAndState(Player, Player.ShaoYue, Player.IdleState, Player.ShaoYue.IdleState);
            }
            else if (AttackInput)
            {
                // Debug.Log("Try Enter Light Attack From Grounded Super State");
                InputHandler.UseAttackInput();
                StateMachine.ChangeState(Player.LightAttackState);
            }
            else if (BlockInput)
            {
                StateMachine.ChangeState(Player.BlockState);
            }
            else if (ChargeInput && Player.ChargeState.CanCharge && !PlayerHealthStats.CheckIfHealthFull)
            {
                StateMachine.ChangeState(Player.ChargeState);
            }
            else if (DashInput && Player.DashState.CanDash)
            {
                // Dash
                StateMachine.ChangeState(Player.DashState);
            }
            else if (JumpInput && Player.JumpState.CanJump)
            {
                // Jump
                StateMachine.ChangeState(Player.JumpState);
            }
            else if (!IsGrounded)
            {
                // InAir
                Player.Animator.SetFloat("yVelocity", Movement.CurrentVelocity.y);
                Player.InAirState.StartJumpCoyoteTime(); // 在 InAirState 設定郊狼時間為 true
                StateMachine.ChangeState(Player.InAirState);
            }
        }
        
        #endregion
    }
}