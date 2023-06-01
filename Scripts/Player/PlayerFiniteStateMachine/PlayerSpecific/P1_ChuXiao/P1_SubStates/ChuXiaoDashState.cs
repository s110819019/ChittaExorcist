using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public class ChuXiaoDashState : ChuXiaoAbilityState<PlayerDashStateData>
    {
        public ChuXiaoDashState(string animationBoolName, ChuXiaoPlayer player) : base(animationBoolName, player)
        {
        }

        #region w/ Dash
        
        // 是否在地面上
        private bool _canDash;
        
        // 最後一次使用 dash 的時間
        private float _lastDashTime;
        
        /// <summary>
        /// 角色在地面上且冷卻時間已經結束
        /// </summary>
        public bool CanDash => _canDash && Time.time >= _lastDashTime + StateData.DashCooldown;
        public void ResetCanDash() => _canDash = true;
        
        #endregion
        
        #region w/ After Image
        
        private Vector2 _lastAfterImagePosition;
        
        private void PlaceAfterImage()
        {
            PlayerAfterImagePool.Instance.GetFromPool();
            _lastAfterImagePosition = Player.transform.position;
        }
        
        private void CheckIfShouldPlaceAfterImage()
        {
            // 根據 PlayerHolder 的位置
            if (Vector2.Distance(Player.transform.parent.transform.position, _lastAfterImagePosition) >= StateData.DistanceBetweenAfterImages)
            {
                PlaceAfterImage();
            }
        }
        
        #endregion

        #region w/ Switch after dash


        private bool _shouldChangeToShaoYue;
        public void SetShouldSwitchToShaoYue()
        {
            _shouldChangeToShaoYue = true;
        }

        #endregion
        
        #region w/ State Workflow
        
        public override void Enter()
        {
            base.Enter();
            InputHandler.UseDashInput();
            
            _canDash = false;
        
            // TODO: 可能需要再考慮 Dash 設計方式
            Movement.Rigidbody2D.drag = StateData.DashDrag;
            Movement.Rigidbody2D.gravityScale = 0.0f;
        
            PlaceAfterImage();
            
            AudioManager.Instance.PlayOnceAudio(StateData.AudioData);
        }
        
        public override void Exit()
        {
            base.Exit();
        
            // TODO: 可能需要再考慮 Dash 設計方式
            Movement.Rigidbody2D.drag = 0;
            Movement.Rigidbody2D.gravityScale = StateData.InitialGravityScale; // 預設重力
            // InputHandler.UseDashInput();
            _lastDashTime = Time.time;
            _shouldChangeToShaoYue = false;
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;

            Movement.SetVelocityXZeroY(StateData.DashSpeedCurve.Evaluate(Time.time - StartTime) * Movement.FacingDirection);
            
            CheckIfShouldPlaceAfterImage();

            if (_shouldChangeToShaoYue)
            {
                // 時間超過
                if (Time.time >= StartTime + StateData.DashDuration)
                {
                    StateMachine.ChangePlayerAndState(Player, Player.ShaoYue, Player.IdleState, Player.ShaoYue.IdleState);
                }
            }
            
            // 時間超過
            if (Time.time >= StartTime + StateData.DashDuration)
            {
                IsAbilityDone = true;
            }

            // Movement.SetVelocityXZeroY(StateData.DashVelocity * Movement.FacingDirection);
        }
        
        #endregion
    }
}