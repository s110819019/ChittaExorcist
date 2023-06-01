using System;
using UnityEngine;

namespace ChittaExorcist.CharacterCore
{
    /// <summary>
    /// Core System 角色移動
    /// </summary>
    public class Movement : CoreComponent
    {
        public Rigidbody2D Rigidbody2D { get; private set; }
        /// <summary>
        /// 角色面朝方向, 起始預設向右 <br/>
        ///  1 => 向右 <br/>
        /// -1 => 向左
        /// </summary>
        public int FacingDirection { get; private set; }
        public bool CanSetVelocity { get; set; }
        public Vector2 CurrentVelocity { get; private set; }
        private Vector2 _workspace;

        #region w/ Workflow

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            CurrentVelocity = Rigidbody2D.velocity;
            // Debug.Log(transform.parent.parent.name + " is Facing : " + FacingDirection);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            // TODO: 暫時解決角色切換轉向問題
            FacingDirection = Rigidbody2D.transform.rotation == Quaternion.identity ? 1 : -1;
        }

        #endregion
        
        #region w/ Unity Callback Functions

        protected override void Awake()
        {
            base.Awake();
            Rigidbody2D = GetComponentInParent<Rigidbody2D>();
            FacingDirection = 1;
            CanSetVelocity = true;
        }

        #endregion

        #region w/ Flip Functions
        
        // 翻轉
        public void Flip()
        {
            FacingDirection *= -1;
            Rigidbody2D.transform.Rotate(0.0f,180.0f,0.0f);
        }
        
        // 確認是否要翻轉圖像
        public void CheckIfShouldFlip(int xInput)
        {
            if (xInput != 0 && xInput != 1 && xInput != -1)
            {
                Debug.LogWarning(" 傳入確認翻轉函式的 xInput 有誤 ");
            }
            if (xInput != 0 && xInput != FacingDirection)
            {
                Flip();
            }
        }

        // public void UpdateFacingDirection(int newFacingDirection)
        // {
        //     if (FacingDirection != newFacingDirection && newFacingDirection != 0)
        //     {
        //         FacingDirection = newFacingDirection;
        //     }
        // }

        // private void CheckFlipDirectionWithRotate()
        // {
        //     if (Rigidbody2D.transform.rotation != Quaternion.identity && FacingDirection == 1)
        //     {
        //         FacingDirection = -1;
        //     }
        // }

        #endregion

        #region w/ Velocity Functions
        
        // 確認是否能設定速度
        private void SetFinalVelocity()
        {
            if (!CanSetVelocity) return;
            Rigidbody2D.velocity = _workspace;
            CurrentVelocity = _workspace;
        }
        
        // 根據角度與方向設定速度
        /// <param name="velocity">速度</param>
        /// <param name="angle">角度</param>
        /// <param name="direction">方向</param>
        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            _workspace.Set(angle.x * velocity * direction, angle.y * velocity);
            SetFinalVelocity();
        }
        
        // 速度皆為零
        public void SetVelocityZero()
        {
            _workspace = Vector2.zero;
            SetFinalVelocity();
        }
        
        // x 速度
        public void SetVelocityX(float velocityX)
        {
            _workspace.Set(velocityX, CurrentVelocity.y);
            SetFinalVelocity();
        }
        
        // y 速度
        public void SetVelocityY(float velocityY)
        {
            _workspace.Set(CurrentVelocity.x, velocityY);
            SetFinalVelocity();
        }
        
        // 只設定 x 並設定 y 為零
        public void SetVelocityXZeroY(float velocityX)
        {
            _workspace.Set(velocityX, 0);
            SetFinalVelocity();
        }
        
        #endregion
    }
}
