using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E1_HitState : E_HitState<Enemy1>
    {
        public E1_HitState(string animationBoolName, Enemy1 enemy, ED_HitStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
            // 1. Look for Player
        }
        
        #region w/ State Workflow

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // if (IsBeatenTimeOver)
            // {
            //     // _enemy.LookForPlayerState.SetTurnImmediately(true);
            //     StateMachine.ChangeState(Enemy.LookForPlayerState);
            // }
            if (IsAnimationFinished)
            {
                // _enemy.LookForPlayerState.SetTurnImmediately(true);
                StateMachine.ChangeState(Enemy.LookForPlayerState);
            }
        }

        #endregion
    }
}