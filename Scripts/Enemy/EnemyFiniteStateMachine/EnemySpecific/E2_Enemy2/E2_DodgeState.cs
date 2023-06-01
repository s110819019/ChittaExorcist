using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E2_DodgeState : E_DodgeState<Enemy2>
    {
        public E2_DodgeState(string animationBoolName, Enemy2 enemy, ED_DodgeStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
        }
        
        #region w/ State Workflow

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsDodgeOver)
            {
                if (IsPlayerInMaxAggroRange && ShouldPerformCloseRangeAction)
                {
                    // MeleeAttack
                    StateMachine.ChangeState(Enemy.MeleeAttackState);
                }
                else if (IsPlayerInMaxAggroRange && !ShouldPerformCloseRangeAction)
                {
                    // RangedAttack
                    StateMachine.ChangeState(Enemy.RangedAttackState);
                }
                else if (!IsPlayerInMaxAggroRange)
                {
                    // LookForPlayer
                    StateMachine.ChangeState(Enemy.LookForPlayerState);
                }
            }
            else if (!IsLedgeVerticalBack)
            {
                StateMachine.ChangeState(Enemy.IdleState);
            }
        }    

        #endregion
    }
}