using UnityEngine;

using ChittaExorcist.CharacterCore;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public abstract class ChuXiaoAbilityState<T> : ChuXiaoPlayerState<T> where T : PlayerStateData
    {
        protected ChuXiaoAbilityState(string animationBoolName, ChuXiaoPlayer player) : base(animationBoolName, player)
        {
            // Light Attack ?
            
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
        
        // Player Health
        protected PlayerHealthStats PlayerHealthStats => _playerHealthStats ? _playerHealthStats : Core.GetCoreComponent(out _playerHealthStats);
        private PlayerHealthStats _playerHealthStats;
        
        protected PlayerMaterialManager PlayerMaterialManager => _playerMaterialManager ? _playerMaterialManager : Core.GetCoreComponent(out _playerMaterialManager);
        private PlayerMaterialManager _playerMaterialManager;
        
        // Damage
        protected PlayerDamageReceiver PlayerDamageReceiver => _playerDamageReceiver ? _playerDamageReceiver : Core.GetCoreComponent(out _playerDamageReceiver);
        private PlayerDamageReceiver _playerDamageReceiver;
        
        #endregion

        #region w/ Variables

        // Input
        protected int XInput;
        protected int YInput;
        
        protected bool JumpInput;
        protected bool DashInput;
        protected bool AttackInput;

        protected bool BlockInput;
        
        protected bool ChargeInput;

        // Check
        protected bool IsGrounded;
        protected bool IsTouchingWall;
        protected bool IsGroundLong;

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
            IsGroundLong = CollisionSenses.GroundLong;
        }

        public override void DoInput()
        {
            base.DoInput();

            XInput = InputHandler.NormalizedXInput;
            YInput = InputHandler.NormalizedYInput;
            JumpInput = InputHandler.JumpInput;
            DashInput = InputHandler.DashInput;
            AttackInput = InputHandler.AttackInput;
            ChargeInput = InputHandler.ChargeInput;
            BlockInput = InputHandler.BlockInput;
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


            if (IsGrounded && AttackInput && Player.JumpState.CanJump)
            {
                // TODO: 條件與 ability 衝突打架問題
                // Debug.Log("Try Enter Light Attack From Ability Super State");
                InputHandler.UseAttackInput();

                StateMachine.ChangeState(Player.LightAttackState);
            }
            else if (IsGrounded && Movement.CurrentVelocity.y < 0.01f)
            {
                // Idle
                StateMachine.ChangeState(Player.IdleState);
            }
            else
            {
                Player.Animator.SetFloat("yVelocity", Movement.CurrentVelocity.y);
                // InAir
                StateMachine.ChangeState(Player.InAirState);
            }
        }

        #endregion
    }
}