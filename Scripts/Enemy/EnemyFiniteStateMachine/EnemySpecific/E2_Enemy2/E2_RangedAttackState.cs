using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E2_RangedAttackState : E_RangedAttackState<Enemy2>
    {
        public E2_RangedAttackState(string animationBoolName, Enemy2 enemy, ED_RangedAttackStateSO stateData, Transform attackPosition) : base(animationBoolName, enemy, stateData, attackPosition)
        {
        }
        
        #region w/ State Workflow

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsAnimationFinished)
            {
                if (IsPlayerInMinAggroRange)
                {
                    StateMachine.ChangeState(Enemy.PlayerDetectedState);
                }
                else
                {
                    StateMachine.ChangeState(Enemy.LookForPlayerState);
                }
            }
        }    

        #endregion
    }
}