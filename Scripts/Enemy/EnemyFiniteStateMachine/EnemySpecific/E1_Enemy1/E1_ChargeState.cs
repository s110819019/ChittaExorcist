using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E1_ChargeState : E_ChargeState<Enemy1>
    {
        public E1_ChargeState(string animationBoolName, Enemy1 enemy, ED_ChargeStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // 1. Melee Attack
            // 2. Look for Player
            // 3 - 1. Player Detected
            // 3 - 2. Look for Player
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
            else if (ShouldPerformCloseRangeAction && !Enemy.MeleeAttackState.CanAttack)
            {
                // PlayerDetected
                StateMachine.ChangeState(Enemy.PlayerDetectedState);
            }
            else if (!IsDetectingLedge || IsDetectingWall)
            {
                // LookForPlayer
                StateMachine.ChangeState(Enemy.LookForPlayerState);
            }
            else if (IsChargeTimeOver)
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