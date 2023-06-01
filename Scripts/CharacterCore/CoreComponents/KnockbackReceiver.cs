using UnityEngine;

using ChittaExorcist.Common.Interfaces;
using ChittaExorcist.EventChannel;

namespace ChittaExorcist.CharacterCore
{
    public class KnockbackReceiver : CoreComponent, IKnockbackable
    {
        [SerializeField] private float maxKnockbackTime = 0.2f;
        
        #region w/ Events

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
        }

        #endregion
        
        #region w/ Variables

        private float _knockbackStartTime;
        private bool _isKnockbackActive;

        #endregion

        #region w/ Core Components

        private CoreComp<Movement> _movement;
        private CoreComp<CollisionSenses> _collisionSenses;

        #endregion
        
        #region w/ Knockback Interface

        public void Knockback(Vector2 angle, float strength)
        {
            if (_movement.Comp == null)
            {
                Debug.Log("無法取得 Movement Comp");
            }

            _movement.Comp.SetVelocity(strength, angle, -_movement.Comp.FacingDirection);
            // _movement.Comp.CheckIfShouldFlip(-direction);
            _movement.Comp.CanSetVelocity = false;
            _isKnockbackActive = true;
            _knockbackStartTime = Time.time;
        }
        
        public void Knockback(Vector2 angle, float strength, int direction)
        {
            if (_movement.Comp == null)
            {
                Debug.Log("無法取得 Movement Comp");
            }

            _movement.Comp.SetVelocity(strength, angle, direction);
            // _movement.Comp.CheckIfShouldFlip(-direction);
            _movement.Comp.CanSetVelocity = false;
            _isKnockbackActive = true;
            _knockbackStartTime = Time.time;
        }
        
        public void Knockback(Vector2 angle, float strength, int direction, IParryable parryable)
        {
            Debug.LogWarning("This function fot Entity not finish");
        }

        #endregion

        #region w/ Knockback

        private void CheckKnockback()
        {
            if (_movement.Comp == null)
            {
                Debug.Log("無法取得 Movement Comp");
            }

            if (_collisionSenses.Comp == null)
            {
                Debug.Log("無法取得 CollisionSenses Comp");
            }
            
            // 正在 Knockback 中 and ((速度 y 為零 and 碰到地面) or 超過最大 knockback time)
            if (_isKnockbackActive && ((_movement.Comp.CurrentVelocity.y <= 0.01f && _collisionSenses.Comp.Ground) ||
                                       Time.time >= _knockbackStartTime + maxKnockbackTime))
            {
                _isKnockbackActive = false;
                _movement.Comp.CanSetVelocity = true;
            }
        }

        #endregion

        #region w/ Work flow

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            CheckKnockback();
        }

        #endregion

        #region w/ Unity Callback Function

        protected override void Awake()
        {
            base.Awake();

            _movement = new CoreComp<Movement>(Core);
            _collisionSenses = new CoreComp<CollisionSenses>(Core);
        }

        #endregion
    }
}