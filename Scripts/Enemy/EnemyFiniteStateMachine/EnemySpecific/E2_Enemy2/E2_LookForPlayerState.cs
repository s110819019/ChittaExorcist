using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E2_LookForPlayerState : E_LookForPlayerState<Enemy2>
    {
        public E2_LookForPlayerState(string animationBoolName, Enemy2 enemy, ED_LookForPlayerStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // 1. PlayerDetected
            // 2. Move
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
            else if (IsAllTurnsTimeOver)
            {
                // Move
                StateMachine.ChangeState(Enemy.MoveState);
            }
        }

        #endregion
    }
}