using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E2_StunState : E_StunState<Enemy2>
    {
        public E2_StunState(string animationBoolName, Enemy2 enemy, ED_StunStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // 1. PlayerDetected
            // 2. LookForPlayer
        }
        
        #region w/ State Workflow

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsStunTimeOver)
            {
                if (IsPlayerInMinAggroRange)
                {
                    // PlayerDetected
                    StateMachine.ChangeState(Enemy.PlayerDetectedState);
                }
                else
                {
                    // LookForPlayer
                    StateMachine.ChangeState(Enemy.LookForPlayerState);
                }
            }
        }    

        #endregion
    }
}