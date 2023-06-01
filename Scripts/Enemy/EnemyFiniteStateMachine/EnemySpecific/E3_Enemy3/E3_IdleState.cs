using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E3_IdleState : E_IdleState<Enemy3>
    {
        public E3_IdleState(string animationBoolName, Enemy3 enemy, ED_IdleStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
        }
        
        #region w/ State Workflow
    
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Enemy.MeleeAttackState.IsAttackInCooldownAfterGetParried)
            {
                return;
            }
            
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