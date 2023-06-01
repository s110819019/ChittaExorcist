using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public class ShaoYueMoveState : ShaoYueGroundedState<PlayerMoveStateData>
    {
        public ShaoYueMoveState(string animationBoolName, ShaoYuePlayer player) : base(animationBoolName, player)
        {
            // 1. Idle
        }
        
        #region w/ Move

        private int _currentXInput;
        private float _currentVelocityX;
        private bool _isTurn;

        public void SetCurrentVelocityX(float lastInAirMovementVelocity)
        {
            switch (lastInAirMovementVelocity)
            {
                case < 0:
                    _currentVelocityX = Mathf.Abs(lastInAirMovementVelocity) >= StateData.MovementVelocity
                        ? -StateData.MovementVelocity
                        : lastInAirMovementVelocity;
                    break;
                case > 0:
                    _currentVelocityX = lastInAirMovementVelocity >= StateData.MovementVelocity
                        ? StateData.MovementVelocity
                        : lastInAirMovementVelocity;
                    break;
                case 0:
                    _currentVelocityX = 0;
                    break;
            }
        }

        private void CheckCurrentVelocity(int xInput)
        {
            if (_currentXInput == 0)
            {
                Debug.LogWarning("MoveState 的 Current XInput 不應設置為 0");
            }
            
            // 移動方向不同
            if (_currentXInput == -xInput)
            {
                _isTurn = !_isTurn;
                _currentXInput = xInput;
            }
            
            // 轉身移動中(減速)
            if (_isTurn)
            {
                // xInput = 0 套用正常減速度, 否則套用轉身減速度
                _currentVelocityX = Mathf.MoveTowards(_currentVelocityX, 0,
                    (xInput == 0 ? StateData.Deceleration : StateData.TurnDeceleration) * Time.deltaTime);
                // 速度為零取消轉身移動狀態
                if (_currentVelocityX == 0.0f)
                {
                    _isTurn = false;
                }
            }
            // 正常移動中
            else
            {
                // 正常加減速度
                _currentVelocityX = Mathf.MoveTowards(_currentVelocityX, xInput * StateData.MovementVelocity,
                    (xInput == 0 ? StateData.Deceleration : StateData.Acceleration) * Time.deltaTime);
            }
        }

        #endregion
        
        #region w/ State Workflow

        public override void Enter()
        {
            base.Enter();
            _currentXInput = InputHandler.NormalizedXInput;
        }

        public override void Exit()
        {
            base.Exit();
            _currentVelocityX = 0;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;

            // 確認是否要翻轉圖像
            Movement.CheckIfShouldFlip(XInput);

            // TODO: 待確定是否使用加速度
            if (StateData.UseAcceleration)
            {
                // 確認速度
                CheckCurrentVelocity(XInput);
                
                // 此處不用乘以 x input, 已在 CheckCurrentVelocity 確認過
                Movement.SetVelocityX(_currentVelocityX);
                
                // 無速度且輸入0
                if (XInput == 0 && _currentVelocityX == 0.0f)
                {
                    // Idle
                    StateMachine.ChangeState(Player.IdleState);
                }                
            }
            else
            {
                Movement.SetVelocityX(XInput * StateData.MovementVelocity);
                
                if (XInput == 0)
                {
                    // Idle
                    StateMachine.ChangeState(Player.IdleState);
                }
            }
        }

        #endregion
    }
}