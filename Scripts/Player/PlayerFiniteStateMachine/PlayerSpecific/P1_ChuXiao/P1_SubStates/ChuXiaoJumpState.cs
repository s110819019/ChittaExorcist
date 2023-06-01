using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public class ChuXiaoJumpState : ChuXiaoAbilityState<PlayerJumpStateData>
    {
        public ChuXiaoJumpState(string animationBoolName, ChuXiaoPlayer player) : base(animationBoolName, player)
        {
            // To In Air
        }
        
        #region w/ Jump

        private bool _isJumpPrepare;
        private int _amountOfJumpsLeft;
        public bool CanJump => _amountOfJumpsLeft > 0;
        public void ResetAmountOfJumpsLeft() => _amountOfJumpsLeft = StateData.AmountOfJumps;
        public void DecreaseAmountOfJumpsLeft() => _amountOfJumpsLeft--;
        private void Jump()
        {
            // if (IsAbilityDone) return;
            
            if (!Movement)
            {
                Debug.LogWarning(" 無法取得 Movement ");
                return;
            }
            
            Movement.SetVelocityY(StateData.JumpVelocity);
            
            IsAbilityDone = true;
            DecreaseAmountOfJumpsLeft();            // 減少跳躍次數
            Player.InputHandler.UseJumpInput();     // 設定 JumpInput 為 false
        
            Player.InAirState.SetIsJumping();       // 在 InAirState 設定正在跳躍 _isJumping 用來實現不同的跳躍高度
            Player.InAirState.EndJumpCoyoteTime();  // 結束 JumpCoyoteTime 的判斷，因為已經跳躍過了，這樣才能接續二段跳
        }
        #endregion
    
        #region w/ State Workflow

        public override void Enter()
        {
            base.Enter();
            // Debug.Log("Enter Jump");
            Player.Animator.SetBool("jump", IsGrounded);
            
            // Player.PlayerHolder.PlayPlayerJumpParticle(true);
            
            Jump();
            AudioManager.Instance.PlayOnceAudio(StateData.AudioData);
        }

        public override void Exit()
        {
            base.Exit();
            Player.Animator.SetBool("jump", false);
        }

        #endregion
    }
}