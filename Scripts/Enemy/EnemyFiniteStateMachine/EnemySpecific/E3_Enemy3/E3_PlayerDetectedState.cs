using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E3_PlayerDetectedState : E_PlayerDetectedState<Enemy3>
    {
        public E3_PlayerDetectedState(string animationBoolName, Enemy3 enemy, ED_PlayerDetectedStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
        }
        
        #region w/ State Workflow

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (ShouldPerformLongRangeAction || ShouldPerformCloseRangeAction)
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
                // if (Movement) Movement.Flip();
                // StateMachine.ChangeState(_enemy.MoveState);
                Enemy.IdleState.SetFlipAfterIdle(true); // 走到邊緣 須返回走
                StateMachine.ChangeState(Enemy.IdleState);
            }
        }

        #endregion
    }
}