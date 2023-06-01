using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E4_MeleeAttackState : E_MeleeAttackState<Enemy4>
    {
        public E4_MeleeAttackState(string animationBoolName, Enemy4 enemy, ED_MeleeAttackStateSO stateData, Transform attackPosition) : base(animationBoolName, enemy, stateData, attackPosition)
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

            // if (ShouldReactAttackParried)
            // {
            //     KnockbackReceiver.Knockback(Enemy.ParriedDetails.ParriedKnockbackDetails.KnockbackAngle,
            //         Enemy.ParriedDetails.ParriedKnockbackDetails.KnockbackStrength,
            //         Enemy.ParriedDetails.ParriedKnockbackDetails.KnockbackDirection);
            //     StateMachine.ChangeState(Enemy.IdleState);
            //     return;
            // }
            
            if (!IsAnimationFinished) return;
        
            StateMachine.ChangeState(Enemy.IdleState);
        }    

        #endregion
    }
}