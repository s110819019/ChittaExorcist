using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E3_DeadState : E_DeadState<Enemy3>
    {
        public E3_DeadState(string animationBoolName, Enemy3 enemy, ED_DeadStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
        }
    }
}