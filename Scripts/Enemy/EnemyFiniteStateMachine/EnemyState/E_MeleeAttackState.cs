using UnityEngine;

using ChittaExorcist.CharacterCore;
using ChittaExorcist.Common.Interfaces;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E_MeleeAttackState<T1> : E_AttackState<T1, ED_MeleeAttackStateSO> where T1 : Enemy
    {
        public E_MeleeAttackState(string animationBoolName, T1 enemy, ED_MeleeAttackStateSO stateData, Transform attackPosition) : base(animationBoolName, enemy, stateData, attackPosition)
        {
            attackPosition.TryGetComponent(out _collider);
            attackPosition.TryGetComponent(out _parryableAttack);

            EventHandler.OnMeleeAttack += TriggerAttack;
            EventHandler.OnStartParryWindows += HandleStartParryWindows;
            EventHandler.OnStopParryWindows += HandleStopParryWindows;
            EventHandler.OnStartAttackReact += OnStartAttackReact;
        }
        
        #region w/ Core Components
    
        // ParticleManager
        protected ParticleManager ParticleManager => _particleManager ? _particleManager : Core.GetCoreComponent(out _particleManager);
        private ParticleManager _particleManager;
        
        // Poise
        protected PoiseReceiver PoiseReceiver => _poiseReceiver ? _poiseReceiver : Core.GetCoreComponent(out _poiseReceiver);
        private PoiseReceiver _poiseReceiver;
        
        public KnockbackReceiver KnockbackReceiver =>
            _knockbackReceiver ? _knockbackReceiver : Core.GetCoreComponent(out _knockbackReceiver);
        private KnockbackReceiver _knockbackReceiver;
        
        #endregion

        #region w/ Components

        private readonly Collider2D _collider;
        private readonly EnemyParryableAttack _parryableAttack;

        #endregion

        #region w/ Variables

        private float _lastAttackTime;

        private float _attackGetParriedStartTime;
        // private bool _canAttack;
        public bool CanAttack => Time.time >= _lastAttackTime + StateData.attackCooldown && !IsAttackInCooldownAfterGetParried;

        protected bool CanAttackReact;
        protected bool IsAttackGetParried;

        public bool IsAttackInCooldownAfterGetParried => IsAttackGetParried &&
                                                            Time.time < _attackGetParriedStartTime +
                                                            StateData.attackCooldownAfterGetParried;

        protected bool ShouldReactAttackParried => CanAttackReact && IsAttackGetParried;

        private void OnStartAttackReact()
        {
            CanAttackReact = true;
        }
        
        #endregion
        
        #region w/ Attack

        public override void TriggerAttack()
        {
            base.TriggerAttack();

            // _parryableAttack.IsSceneTrap = true;
            
            Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(AttackPosition.position, StateData.attackRadius, StateData.whatIsPlayer);

            foreach (var item in detectedObjects)
            {
                if (item.TryGetComponent(out IDamageable damageable))
                {
                    if (_parryableAttack != null)
                    {
                        damageable.Damage(StateData.attackDamage, _parryableAttack);
                    }
                    else
                    {
                        damageable.Damage(StateData.attackDamage);
                    }
                }
                
                if (item.TryGetComponent(out IKnockbackable knockbackable))
                {
                    if (_parryableAttack != null)
                    {
                        knockbackable.Knockback(StateData.knockBackAngle, StateData.knockBackStrength,
                            Movement.FacingDirection, _parryableAttack);
                    }
                    else
                    {
                        knockbackable.Knockback(StateData.knockBackAngle, StateData.knockBackStrength,
                            Movement.FacingDirection);
                    }
                }
            }

            if (_parryableAttack != null)
            {
                if (_parryableAttack.IsParried)
                {
                    _attackGetParriedStartTime = Time.time;
                    IsAttackGetParried = true;
                }
            }
            
            // if (Enemy.GetParryShouldKnockback)
            // {
            //     KnockbackReceiver.Knockback(Enemy.ParryKnockbackDetails.KnockbackAngle,
            //         Enemy.ParryKnockbackDetails.KnockbackStrength,
            //         Enemy.ParryKnockbackDetails.KnockbackDirection);
            //     Enemy.SetGetParryShouldKnockback(false);
            // }
        }

        public override void TriggerAttackEffect()
        {
            base.TriggerAttackEffect();

            // if (StateData.hitParticle != null)
            // {
            //     ParticleManager.StartParticle(StateData.hitParticle, StateData.hitParticlePositionOffset);
            // }
        }

        #endregion

        #region w/ Parryable Collider

        private void HandleStartParryWindows()
        {
            _collider.enabled = true;
        }

        private void HandleStopParryWindows()
        {
            _collider.enabled = false;
        }

        #endregion
        
        #region w/ State Workflow

        public override void Enter()
        {
            base.Enter();

            IsAttackGetParried = false;
            CanAttackReact = false;
            
            if (_parryableAttack != null)
            {
                _parryableAttack.SetAttackTransform(Enemy.transform);
            }
        }

        public override void Exit()
        {
            base.Exit();
            _lastAttackTime = Time.time;
            if (_collider != null)
            {
                _collider.enabled = false;
            }

            if (_parryableAttack != null)
            {
                _parryableAttack.ResetParryCheck();
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

        }

        #endregion
    }
}