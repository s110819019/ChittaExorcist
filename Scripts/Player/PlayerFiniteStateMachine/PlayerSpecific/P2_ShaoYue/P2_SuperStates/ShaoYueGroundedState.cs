using UnityEngine;

using ChittaExorcist.CharacterCore;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public abstract class ShaoYueGroundedState<T> : ShaoYuePlayerState<T> where T : PlayerStateData
    {
        protected ShaoYueGroundedState(string animationBoolName, ShaoYuePlayer player) : base(animationBoolName, player)
        {
            // 1. Jump
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
        protected bool BlockInput;
        protected bool AttackInput;
        
        // TODO: Charge Input 或是另外設置大招按鍵?
        protected bool ChargeInput;
        

        // TODO: How Switch
        protected bool SwitchInput;
        
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
            BlockInput = InputHandler.BlockInput;
            AttackInput = InputHandler.AttackInput;
            SwitchInput = InputHandler.SwitchInput;

            ChargeInput = InputHandler.ChargeInput;
        }
        
        public override void Enter()
        {
            base.Enter();
            
            // 重設跳躍次數
            Player.JumpState.ResetAmountOfJumpsLeft();
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!Movement)
            {
                Debug.LogWarning(" 無法取得 Movement ");
                return;
            }
            // if (!PlayerStats)
            // {
            //     Debug.LogWarning(" 無法取得 PlayerStats ");
            //     return;
            // }
            if (PlayerManaStats.ManaIsZero())
            {
                StateMachine.ChangePlayerAndState(Player, Player.ChuXiao, Player.IdleState, Player.ChuXiao.IdleState);
                return;
            }
            
            // if (SwitchInput)
            // {
            //     InputHandler.UseSwitchInput();
            //     StateMachine.ChangePlayerAndState(Player, Player.ChuXiao, Player.IdleState, Player.ChuXiao.IdleState);
            // }
            // else if (ChargeInput)
            // {
            //     InputHandler.UseChargeInput();
            //     StateMachine.ChangeState(Player.HeavyAttackState);
            // }
            if (SwitchInput)
            {
                InputHandler.UseSwitchInput();
                StateMachine.ChangeState(Player.HeavyAttackState);
            }
            else if (YInput == 1 && AttackInput && PlayerManaStats.CheckManaCost(Player.LightAttackState.Ability.ManaRequireCost()))
            {
                // PlayerStats.DecreaseMana(5.0f);
                InputHandler.UseAttackInput();
                StateMachine.ChangeState(Player.LightUpAttackState);
            }
            else if (AttackInput && PlayerManaStats.CheckManaCost(Player.LightAttackState.Ability.ManaRequireCost()))
            {
                // PlayerStats.DecreaseMana(5.0f);
                InputHandler.UseAttackInput();
                StateMachine.ChangeState(Player.LightAttackState);
            }
            else if (JumpInput && Player.JumpState.CanJump)
            {
                // Jump
                StateMachine.ChangeState(Player.JumpState);
            }
            else if (!IsGrounded)
            {
                // InAir
                Player.InAirState.StartJumpCoyoteTime(); // 在 InAirState 設定郊狼時間為 true
                StateMachine.ChangeState(Player.InAirState);
            }
            else if (BlockInput)
            {
                StateMachine.ChangeState(Player.BlockState);
            }
        }
        
        #endregion
    }
}