using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E1_LookForPlayerState : E_LookForPlayerState<Enemy1>
    {
        public E1_LookForPlayerState(string animationBoolName, Enemy1 enemy, ED_LookForPlayerStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // 1. Player Detected
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