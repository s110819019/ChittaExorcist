using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E3_ChargeState : E_ChargeState<Enemy3>
    {
        public E3_ChargeState(string animationBoolName, Enemy3 enemy, ED_ChargeStateSO stateData) : base(animationBoolName, enemy, stateData)
        {
        }
        
        #region w/ Charge Settings After Melee Attack

        private bool _hasMeleeAttack;
        public void SetHasMeleeAttack() => _hasMeleeAttack = true;
        private void CheckChargeTimeAfterMeleeAttack()
        {
            if (Time.time >= StartTime + StateData.chargeTimeAfterMeleeAttack && _hasMeleeAttack)
            {
                IsChargeTimeOver = true;
            }
        }

        #endregion

        #region w/ State Workflow

        public override void Exit()
        {
            base.Exit();
            _hasMeleeAttack = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            CheckChargeTimeAfterMeleeAttack();
        
            if (ShouldPerformCloseRangeAction && Enemy.MeleeAttackState.CanAttack)
            {
                // MeleeAttack
                StateMachine.ChangeState(Enemy.MeleeAttackState);
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