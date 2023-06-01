using UnityEngine;

using ChittaExorcist.PlayerSettings.PlayerAbilitySystem;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public class ChuXiaoSubAbilityState : ChuXiaoAbilityState<PlayerSubAbilityStateData>
    {
        private readonly PlayerAbility _ability;
        
        public ChuXiaoSubAbilityState(string animationBoolName, ChuXiaoPlayer player, PlayerAbility ability) : base(animationBoolName, player)
        {
            // Early Out ?
            // 1. Move
            // 2. Jump
            // 3. Dash
            
            _ability = ability;

            // TODO: 待學習是否有先後問題和訂閱取消問題
            ability.OnExit += ExitHandler;
            ability.OnEarlyOut += EarlyOutHandler;
        }

        private void ExitHandler()
        {
            AnimationFinishTrigger();
            IsAbilityDone = true;
            // Debug.Log("Sub Ability State ExitHandler");
        }

        private void EarlyOutHandler()
        {
            IsAbilityEarlyOut = true;
        }

        #region w/ State Workflow

        public override void Enter()
        {
            base.Enter();

            // Debug.Log("Sub Ability State Enter");
            _ability.Enter();
            
            Movement.SetVelocityZero();
        }

        public override void Exit()
        {
            base.Exit();
            if (!IsAbilityDone)
            {
                _ability.Exit();
            }
            
            PlayerMaterialManager.InitializeSpriteRenderer(Player.SpriteRenderer);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!IsAbilityEarlyOut) return;

            // if (AttackInput && IsGrounded)
            // {
            //     _ability.Exit();
            //     StateMachine.ChangeState(Player.LightAttackState);
            // }
            // else 
            
            if (XInput == -Movement.FacingDirection && IsGrounded && !AttackInput)
            {
                // Move
                StateMachine.ChangeState(Player.MoveState);
            }
            else if (IsGrounded && JumpInput && Player.JumpState.CanJump)
            {
                // Jump
                StateMachine.ChangeState(Player.JumpState);
            }
            else if (IsGrounded && DashInput && Player.DashState.CanDash)
            {
                // Dash
                StateMachine.ChangeState(Player.DashState);
            }
            else if (BlockInput)
            {
                StateMachine.ChangeState(Player.BlockState);
            }
        }

        #endregion
    }
}