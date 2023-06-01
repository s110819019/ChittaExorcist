using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E4_IdleState : E_IdleState<Enemy4>
    {
        public E4_IdleState(string animationBoolName, Enemy4 enemy, ED_IdleStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
        }
        
        #region w/ State Workflow
    
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsPlayerInMinAggroRange)
            {
                StateMachine.ChangeState(Enemy.PlayerDetectedState);
            }
        }    

        #endregion
    }
}