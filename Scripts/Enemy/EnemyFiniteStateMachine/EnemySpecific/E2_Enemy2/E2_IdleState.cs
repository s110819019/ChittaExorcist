using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E2_IdleState : E_IdleState<Enemy2>
    {
        public E2_IdleState(string animationBoolName, Enemy2 enemy, ED_IdleStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // 1. Move
        }
        
        #region w/ State Workflow

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsIdleTimeOver)
            {
                // Move
                StateMachine.ChangeState(Enemy.MoveState);
            }
        }    

        #endregion
    }
}