using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E2_MoveState : E_MoveState<Enemy2>
    {
        public E2_MoveState(string animationBoolName, Enemy2 enemy, ED_MoveStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // 1. PlayerDetected
            // 2. Idle
        }

        #region w/ State Workflow
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsPlayerInMinAggroRange)
            {
                // PlayerDetected
                StateMachine.ChangeState(Enemy.PlayerDetectedState);
            }
            else if (!IsDetectingLedge || IsDetectingWall)
            {
                // Idle
                Enemy.IdleState.SetFlipAfterIdle(true); // 走到邊緣 須返回走
                StateMachine.ChangeState(Enemy.IdleState);
            }
        }

        #endregion
    }
}