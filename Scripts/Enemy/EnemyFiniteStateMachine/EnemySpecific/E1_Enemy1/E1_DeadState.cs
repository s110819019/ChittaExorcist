using ChittaExorcist.CharacterCore;
using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E1_DeadState : E_DeadState<Enemy1>
    {
        public E1_DeadState(string animationBoolName, Enemy1 enemy, ED_DeadStateSO stateData) : base(animationBoolName, enemy, stateData)
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