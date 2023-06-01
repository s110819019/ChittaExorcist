
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public class ShaoYueIdleState : ShaoYueGroundedState<PlayerIdleStateData>
    {
        public ShaoYueIdleState(string animationBoolName, ShaoYuePlayer player) : base(animationBoolName, player)
        {
            // 1. Move
        }
        
        #region w/ State Workflow
    
        public override void Enter()
        {
            base.Enter();
            // Debug.Log("Idle");
            // Movement.SetVelocityX(0);
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            
            Movement.SetVelocityX(0);
            
            if (XInput != 0)
            {
                // Move
                StateMachine.ChangeState(Player.MoveState);
            }
        }

        #endregion
    }
}