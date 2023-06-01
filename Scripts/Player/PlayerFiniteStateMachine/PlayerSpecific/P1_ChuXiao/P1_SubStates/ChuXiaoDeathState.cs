using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public class ChuXiaoDeathState : ChuXiaoAbilityState<PlayerDeathStateData>
    {
        public ChuXiaoDeathState(string animationBoolName, ChuXiaoPlayer player) : base(animationBoolName, player)
        {
        }
        
        #region w/ State Workflow

        public override void Enter()
        {
            base.Enter();
            PlayerDamageReceiver.DisableHitBox();
        }

        public override void Exit()
        {
            base.Exit();
            PlayerDamageReceiver.EnableHitBox();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        
            Movement.SetVelocityX(0f);

            if (!IsAnimationFinished)
            {
                return;
            }

            if (Player.IsOnDeath)
            {
                Player.ResetOnDeath();
                Player.PlayerHolder.onPlayerDeath.Broadcast();
            }
        }

        #endregion
    }
}