using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E1_PlayerDetectedState : E_PlayerDetectedState<Enemy1>
    {
        public E1_PlayerDetectedState(string animationBoolName, Enemy1 enemy, ED_PlayerDetectedStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // 1. Melee Attack
            // 2. Charge
            // 3. Look for Player
            // 4. Idle
        }
        
        #region w/ State Workflow

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (ShouldPerformCloseRangeAction && Enemy.MeleeAttackState.CanAttack)
            {
                // MeleeAttack
                StateMachine.ChangeState(Enemy.MeleeAttackState);
            }
            else if (ShouldPerformLongRangeAction && !ShouldPerformCloseRangeAction)
            {
                // Charge
                StateMachine.ChangeState(Enemy.ChargeState);
            }
            else if (!IsPlayerInMaxAggroRange && !IsPlayerInMinAggroRange)
            {
                // LookForPlayer
                StateMachine.ChangeState(Enemy.LookForPlayerState);
            }
            else if (!IsDetectingLedge || IsDetectingWall)
            {
                // Idle
                Enemy.IdleState.SetFlipAfterIdle(true); // 走到邊緣 須返回走
                StateMachine.ChangeState(Enemy.IdleState);
            }
        }

        #endregion
    }
}