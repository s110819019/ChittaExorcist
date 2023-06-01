
namespace ChittaExorcist.PlayerSettings.FSM
{
    public class ShaoYueLandState : ShaoYueGroundedState<PlayerLandStateData>
    {
        public ShaoYueLandState(string animationBoolName, ShaoYuePlayer player) : base(animationBoolName, player)
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
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;

            Movement.SetVelocityX(0.0f);

            if (XInput != 0)
            {
                // Move
                Player.MoveState.SetCurrentVelocityX(XInput * _lastInAirMovementVelocity < 0.0f
                    ? 0.0f
                    : _lastInAirMovementVelocity);
                StateMachine.ChangeState(Player.MoveState);
            }
            else if (IsAnimationFinished)
            {
                // Idle
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        #endregion
    }
}