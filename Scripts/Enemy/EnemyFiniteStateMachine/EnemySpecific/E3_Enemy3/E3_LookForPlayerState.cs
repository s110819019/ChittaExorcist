using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E3_LookForPlayerState : E_LookForPlayerState<Enemy3>
    {
        public E3_LookForPlayerState(string animationBoolName, Enemy3 enemy, ED_LookForPlayerStateSO stateData) : base(animationBoolName, enemy, stateData)
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
            else if (IsAllTurnsTimeOver)
            {
                // Move
                StateMachine.ChangeState(Enemy.MoveState);
            }
        }

        #endregion
    }
}