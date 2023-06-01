using ChittaExorcist.Common.Interfaces;
using ChittaExorcist.PlayerSettings.PlayerAbilitySystem;
using UnityEngine;

namespace ChittaExorcist.CharacterCore
{
    public class PlayerKnockbackReceiver : CoreComponent, IKnockbackable
    {
        [SerializeField] private float maxKnockbackTime = 0.2f;

        #region w/ Player Ability

        private AbilityBlock _abilityBlock;

        public void InitializeAbilityBlock(AbilityBlock block)
        {
            _abilityBlock = block;
        }

        private AbilityParry _abilityParry;

        public void InitializeAbilityParry(AbilityParry parry)
        {
            _abilityParry = parry;
        }

        // private bool _isBlockSuccess;
        // private bool _isParrySuccess;
        
        #endregion
        
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

            // TODO: 要判斷力道零嗎
            if (strength == 0)
            {
                return;
            }
            
            _movement.Comp.SetVelocity(strength, angle, -_movement.Comp.FacingDirection);
            _movement.Comp.CanSetVelocity = false;
            _isKnockbackActive = true;
            _knockbackStartTime = Time.time;

            // _isParrySuccess = false;
            // _isBlockSuccess = false;
        }
        
        public void Knockback(Vector2 angle, float strength, int direction)
        {
            if (_movement.Comp == null)
            {
                Debug.Log("無法取得 Movement Comp");
            }

            // TODO: 要判斷力道零嗎
            if (strength == 0)
            {
                return;
            }
            
            _movement.Comp.SetVelocity(strength, angle, direction);
            _movement.Comp.CanSetVelocity = false;
            _isKnockbackActive = true;
            _knockbackStartTime = Time.time;

            // _isParrySuccess = false;
            // _isBlockSuccess = false;
        }
        
        public void Knockback(Vector2 angle, float strength, int direction, IParryable parryable)
        {
            if (parryable != null)
            {
                if (_abilityParry != null)
                {
                    if (parryable.IsParried)
                    {
                        // _isParrySuccess = true;
                        Knockback(angle, _abilityParry.CheckKnockbackStrengthAbsorption(strength), direction);
                        _movement.Comp.CheckIfShouldFlip(-direction);
                        return;
                    }
                }
                
                if (_abilityBlock != null)
                {
                    if (_abilityBlock.CheckPlayerGetDamagedBlockSuccess(parryable.AttackTransform))
                    {
                        // _isBlockSuccess = true;
                        Knockback(angle, _abilityBlock.CheckKnockbackStrengthAbsorption(strength), direction);
                        _movement.Comp.CheckIfShouldFlip(-direction);
                        return;
                    }
                }                
            }

            Knockback(angle, strength, direction);
            
            _movement.Comp.CheckIfShouldFlip(-direction);
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