using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E3_MoveState : E_MoveState<Enemy3>
    {
        public E3_MoveState(string animationBoolName, Enemy3 enemy, ED_MoveStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
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