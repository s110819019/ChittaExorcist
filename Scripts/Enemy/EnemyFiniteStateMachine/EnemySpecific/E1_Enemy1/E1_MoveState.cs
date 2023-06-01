using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E1_MoveState : E_MoveState<Enemy1>
    {
        public E1_MoveState(string animationBoolName, Enemy1 enemy, ED_MoveStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // 1. Player Detected
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