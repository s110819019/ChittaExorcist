using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E4_PlayerDetectedState : E_PlayerDetectedState<Enemy4>
    {
        public E4_PlayerDetectedState(string animationBoolName, Enemy4 enemy, ED_PlayerDetectedStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
        }
        
        #region w/ State Workflow

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // if (!isPlayerInMaxAggroRange)
            // {
            //     enemy.idleState.SetFlipAfterIdle(false);
            //     stateMachine.ChangeState(enemy.idleState);
            // }

            if (ShouldPerformCloseRangeAction)
            {
                // MeleeAttack
                StateMachine.ChangeState(Enemy.MeleeAttackState);
            }
            else if(!IsPlayerInMaxAggroRange)
            {
                StateMachine.ChangeState(Enemy.IdleState);

            }
            // else if (!IsPlayerInMaxAggroRange && !IsPlayerInMinAggroRange)
            // {
            //     // LookForPlayer
            //     StateMachine.ChangeState(_enemy.LookForPlayerState);
            // }
        }

        #endregion
    }
}