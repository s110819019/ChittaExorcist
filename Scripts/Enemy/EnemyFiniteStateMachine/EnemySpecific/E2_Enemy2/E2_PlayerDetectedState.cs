using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E2_PlayerDetectedState : E_PlayerDetectedState<Enemy2>
    {
        public E2_PlayerDetectedState(string animationBoolName, Enemy2 enemy, ED_PlayerDetectedStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // 1. Dodge or MeleeAttack
            // 2. RangedAttack
            // 3. LookForPlayer
        }
        
        #region w/ State Workflow

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (ShouldPerformCloseRangeAction && (Enemy.DodgeState.CanDodge || Enemy.MeleeAttackState.CanAttack))
            {
                if (Enemy.DodgeState.CanDodge)
                {
                    if (IsWallBack || !IsLedgeVerticalBack)
                    {
                        Movement.Flip();
                    }
                    // Dodge
                    StateMachine.ChangeState(Enemy.DodgeState);
                }
                else
                {
                    // MeleeAttack
                    StateMachine.ChangeState(Enemy.MeleeAttackState);                    
                }
            }
            else if (ShouldPerformLongRangeAction)
            {
                // RangedAttack
                StateMachine.ChangeState(Enemy.RangedAttackState);
            }
            else if (!IsPlayerInMaxAggroRange)
            {
                // LookForPlayer
                Enemy.LookForPlayerState.SetTurnImmediately(true);
                StateMachine.ChangeState(Enemy.LookForPlayerState);
            }
            // else
            // {
            //     // TODO: Maybe not good
            //     StateMachine.ChangeState(Enemy.PlayerDetectedState);
            // }
            
            // Debug.Log("E2_PlayerDetected");
        }

        #endregion
    }
}