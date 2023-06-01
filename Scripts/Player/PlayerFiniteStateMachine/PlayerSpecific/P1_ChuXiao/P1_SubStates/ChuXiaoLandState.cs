
using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public class ChuXiaoLandState : ChuXiaoGroundedState<PlayerLandStateData>
    {
        public ChuXiaoLandState(string animationBoolName, ChuXiaoPlayer player) : base(animationBoolName, player)
        {
            // 1. Move
            // 2. Idle
        }

        #region w/ For Move

        private float _lastInAirMovementVelocity;
        public void SetLastInAirVelocity(float lastVelocity) => _lastInAirMovementVelocity = lastVelocity;

        #endregion
        
        #region w/ State Workflow

        public override void Enter()
        {
            base.Enter();
        
            Movement.SetVelocityX(0.0f);
            
            Player.PlayerHolder.PlayPlayerLandParticle(true);
            AudioManager.Instance.PlayAudio(StateData.AudioData);
        }

        public override void Exit()
        {
            base.Exit();
            Player.PlayerHolder.PlayPlayerLandParticle(false);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;

            Movement.SetVelocityX(0.0f);

            if (XInput != 0)
            {
                // Move
                // TODO: 可能太複雜
                Player.MoveState.SetCurrentVelocityX(XInput * _lastInAirMovementVelocity < 0.0f
                    ? 0.0f
                    : _lastInAirMovementVelocity);
                // Player.MoveState.SetCurrentVelocityX(_lastInAirMovementVelocity);
                StateMachine.ChangeState(Player.MoveState);
            }
            else if (IsAnimationFinished)
            {
                // Idle
                StateMachine.ChangeState(Player.IdleState);
            }
            else if (Time.time >= StartTime + StateData.MaxLandingTime)
            {
                Debug.Log("Why Out Land Time ?");
                // Idle
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        #endregion
    }
}