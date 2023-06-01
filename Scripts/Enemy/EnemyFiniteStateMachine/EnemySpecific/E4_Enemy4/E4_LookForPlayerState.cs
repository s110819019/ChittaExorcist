using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E4_LookForPlayerState : E_LookForPlayerState<Enemy4>
    {
        public E4_LookForPlayerState(string animationBoolName, Enemy4 enemy, ED_LookForPlayerStateSO stateData) : base(animationBoolName, enemy, stateData)
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
        }

        #endregion
    }
}