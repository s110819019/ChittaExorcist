using UnityEngine;

using ChittaExorcist.CharacterCore;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public class ChuXiaoInAirState : ChuXiaoPlayerState<PlayerInAirStateData>
    {
        public ChuXiaoInAirState(string animationBoolName, ChuXiaoPlayer player) : base(animationBoolName, player)
        {
            // Air Attack ?
            
            // 1. Land
            // 2. Dash
            // 3. Jump
        }

        #region w/ Core Components

        // Movement
        private Movement _movement;
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);

        // Collision Senses
        private CollisionSenses _collisionSenses;
        protected CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses : Core.GetCoreComponent(out _collisionSenses);
        
        // Player Mana
        protected PlayerManaStats PlayerManaStats => _playerManaStats ? _playerManaStats : Core.GetCoreComponent(out _playerManaStats);
        private PlayerManaStats _playerManaStats;
        
        #endregion

        #region w/ Variables

        // Input
        private int _xInput;
        private int _yInput;
        private bool _jumpInput;
        private bool _jumpInputStop;
        private bool _dashInput;

        private bool _attackInput;

        private bool _switchInput;

        // Check
        private bool _isGrounded;

        private bool _isGroundLong;
        // private bool _isTouchingWall;
        // private bool _isTouchingLedge;

        #endregion

        #region w/ In Air Moving

        private int _currentXInput;
        private float _currentVelocityX;
        private bool _isTurn;

        private void CheckCurrentVelocity(int xInput)
        {
            // 跳躍無移動
            if (_currentXInput == 0)
            {
                if (xInput != 0)
                {
                    _currentXInput = xInput;
                }
                else
                {
                    return;
                }
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
                // Debug.Log("Is Air Turning");
                // xInput = 0 套用正常減速度, 否則套用轉身減速度
                _currentVelocityX = Mathf.MoveTowards(_currentVelocityX, 0,
                    (xInput == 0 ? StateData.AirDeceleration : StateData.AirTurnDeceleration) * Time.deltaTime);
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
                _currentVelocityX = Mathf.MoveTowards(_currentVelocityX, xInput * StateData.AirMovementVelocity,
                    (xInput == 0 ? StateData.AirDeceleration : StateData.AirAcceleration) * Time.deltaTime);
            }

            // TODO: 判定可能不對?
            if (_currentVelocityX == 0.0f && xInput == 0)
            {
                _currentXInput = 0;
            }
        }

        #endregion
        
        #region w/ Jumping

        private float _jumpStartTime;
        private float _fallStartTime;

        private bool _isJumpInputStop;
        private bool _isJumping;
        private bool _isJumpCoyoteTime;

        private bool _hasCutoffJumpSpeed;

        private float JumpDuration => Time.time - _jumpStartTime;
        private float FallDuration => Time.time - _fallStartTime;
        
        public void SetIsJumping()
        {
            _isJumping = true;
            _jumpStartTime = Time.time;
            _isJumpInputStop = false;
            
            Player.PlayerHolder.PlayPlayerJumpParticle(true);
        }
        public void StartJumpCoyoteTime() => _isJumpCoyoteTime = true;
        public void EndJumpCoyoteTime() => _isJumpCoyoteTime = false;
        
        private void CheckVariableJumpHeight()
        {
            if (!_isJumping) return;
            // 跳躍中會繼續下列判斷
            
            // jump 輸入停止
            if (_jumpInputStop)
            {
                // 使用 variable jump height
                _isJumpInputStop = true;
            }
            
            // y 速度為 0 即為非跳躍中
            if (Movement.CurrentVelocity.y <= 0.01f)
            {
                // 使用 fall
                _isJumping = false;
                _fallStartTime = Time.time;
            }
        }
        
        private void CheckYVelocity()
        {
            if (StateData.UseVariableJumpHeight)
            {
                switch (_isJumping)
                {
                    // 非跳躍中
                    case false:
                        // Fall
                        if (StateData.UseFallSpeedCurve)
                        {
                            Movement.SetVelocityY(StateData.FallSpeedCurve.Evaluate(FallDuration));
                        }
                        break;
                    // 跳躍中
                    case true:
                        // variable height 的實現方式, 暫時先將 duration 乘以 jump cutoff 數值當作加快時間
                        // Jump Variable and Full Jump
                        if (StateData.UseJumpUpSpeedCurve)
                        {
                            Movement.SetVelocityY(_isJumpInputStop
                                ? StateData.JumpUpSpeedCurve.Evaluate(JumpDuration * StateData.JumpCutoff)
                                : StateData.JumpUpSpeedCurve.Evaluate(JumpDuration));
                        }
                        else if (_isJumpInputStop && !_hasCutoffJumpSpeed)
                        {
                            Movement.SetVelocityY(Movement.CurrentVelocity.y * (1 / StateData.JumpCutoff));
                            _hasCutoffJumpSpeed = true;
                        }
                        break;
                }                
            }
            else
            {
                if (StateData.UseJumpUpSpeedCurve)
                {
                    if (_isJumping)
                    {
                        Movement.SetVelocityY(StateData.JumpUpSpeedCurve.Evaluate(JumpDuration));
                    }
                }
                
                if (StateData.UseFallSpeedCurve)
                {
                    if (!_isJumping)
                    {
                        Movement.SetVelocityY(StateData.FallSpeedCurve.Evaluate(FallDuration));
                    }
                }
            }
        }

        private void CheckJumpCoyoteTime()
        {
            if (!StateData.UseCoyoteTime) return;

            if (!_isJumpCoyoteTime || !(Time.time >= StartTime + StateData.CoyoteTime)) return;
            _isJumpCoyoteTime = false;
            Player.JumpState.DecreaseAmountOfJumpsLeft();
        }

        #endregion

        #region w/ State Workflow

        public override void DoCheck()
        {
            base.DoCheck();
            if (!CollisionSenses)
            {
                Debug.LogWarning(" 無法取得 CollisionSenses ");
                return;
            }
            _isGrounded = CollisionSenses.Ground;
            _isGroundLong = CollisionSenses.GroundLong;
        }

        public override void DoInput()
        {
            base.DoInput();

            _xInput = InputHandler.NormalizedXInput;
            _yInput = InputHandler.NormalizedYInput;
            _jumpInput = InputHandler.JumpInput;
            _jumpInputStop = InputHandler.JumpInputStop;
            _dashInput = InputHandler.DashInput;
            _attackInput = InputHandler.AttackInput;
            _switchInput = InputHandler.SwitchInput;
        }

        public override void Enter()
        {
            base.Enter();
            // Debug.Log("Enter In Air");

            _currentXInput = InputHandler.NormalizedXInput;
            _fallStartTime = Time.time;
            _hasCutoffJumpSpeed = false;
            // Player.Animator.SetFloat("yVelocity", Movement.CurrentVelocity.y);

            if (!StateData.UseCoyoteTime && _isJumpCoyoteTime)
            {
                Player.JumpState.DecreaseAmountOfJumpsLeft();
            }
        }

        public override void Exit()
        {
            base.Exit();
            _currentVelocityX = 0;
            // 需要嗎?
            _isJumping = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!Movement)
            {
                Debug.LogWarning(" 無法取得 Movement ");
                return;
            }

            CheckJumpCoyoteTime(); // 郊狼時間
            CheckVariableJumpHeight(); // 不同跳躍高度

            if (Time.time >= StartTime + StateData.JumpParticleTime)
            {
                Player.PlayerHolder.PlayPlayerJumpParticle(false);
            }
            
            
            // if (_attackInput && PlayerManaStats.ManaIsFull())
            // {
            //     InputHandler.UseAttackInput();
            //     Player.ShaoYue.AirLightAttackState.Ability.SetIsFirstAttack();
            //     StateMachine.ChangePlayerAndState(Player, Player.ShaoYue, Player.IdleState, Player.ShaoYue.AirLightAttackState);
            // }
            // TODO: 會跟跳躍打架
            if (_attackInput && !Player.JumpState.CanJump && !_isGroundLong)
            {
                InputHandler.UseAttackInput();
                StateMachine.ChangeState(Player.AirLightAttackState);
            }
            if (_switchInput && PlayerManaStats.ManaIsFull() && Player.PlayerHolder.CanSwitchCharacter)
            {
                InputHandler.UseSwitchInput();
                StateMachine.ChangePlayerAndState(Player, Player.ShaoYue, Player.IdleState, Player.ShaoYue.IdleState);
            }
            else if (_isGrounded && Movement.CurrentVelocity.y < 0.01f)
            {
                // Land
                // TODO: 可能太複雜
                Player.LandState.SetLastInAirVelocity(_currentVelocityX);
                StateMachine.ChangeState(Player.LandState);
            }
            else if (_dashInput && Player.DashState.CanDash)
            {
                // Dash
                StateMachine.ChangeState(Player.DashState);
            }
            else if (_jumpInput && Player.JumpState.CanJump)
            {
                // Jump
                StateMachine.ChangeState(Player.JumpState);
            }
            else
            {
                // 確認是否要翻轉圖像
                Movement.CheckIfShouldFlip(_xInput);

                if (StateData.UseAcceleration)
                {
                    // 確認速度
                    CheckCurrentVelocity(_xInput);
                    
                    // 此處不用乘以 x input, 已在 CheckInAirCurrentVelocity 確認過
                    Movement.SetVelocityX(_currentVelocityX);                    
                }
                else
                {
                    Movement.SetVelocityX(_xInput * StateData.AirMovementVelocity);
                }
                
                CheckYVelocity();
                
                Player.Animator.SetFloat("yVelocity", Movement.CurrentVelocity.y);
            }
        }

        #endregion
    }
}