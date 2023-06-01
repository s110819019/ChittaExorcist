using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E2_DeadState : E_DeadState<Enemy2>
    {
        public E2_DeadState(string animationBoolName, Enemy2 enemy, ED_DeadStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
        }
        
        #region w/ State Workflow

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }    

        #endregion
    }
}