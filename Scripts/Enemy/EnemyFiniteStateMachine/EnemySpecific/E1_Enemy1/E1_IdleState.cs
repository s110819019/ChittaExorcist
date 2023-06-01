using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E1_IdleState : E_IdleState<Enemy1>
    {
        public E1_IdleState(string animationBoolName, Enemy1 enemy, ED_IdleStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // 1. Move
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
            else if (IsIdleTimeOver)
            {
                // Move
                StateMachine.ChangeState(Enemy.MoveState);
            }
        }    

        #endregion
    }
}