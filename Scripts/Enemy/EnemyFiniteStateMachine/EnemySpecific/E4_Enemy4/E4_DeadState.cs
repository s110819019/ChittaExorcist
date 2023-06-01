using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E4_DeadState : E_DeadState<Enemy4>
    {
        public E4_DeadState(string animationBoolName, Enemy4 enemy, ED_DeadStateSO stateData) : base(animationBoolName, enemy, stateData)
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