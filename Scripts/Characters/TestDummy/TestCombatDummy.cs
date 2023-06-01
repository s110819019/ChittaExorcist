using System;
using ChittaExorcist.Common.Interfaces;
using UnityEngine;

namespace ChittaExorcist.Characters
{
    public class TestCombatDummy : MonoBehaviour, IDamageable, IKnockbackable
    {
        public bool canKnockback;
        
        #region w/ Variables

        private Vector2 _workspace;

        #endregion
        
        #region w/ Components

        private Rigidbody2D _rigidbody2D;
        private Animator _animator;

        #endregion

        #region w/ Unity Callback Functions

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        #endregion
        
        public void Damage(float amount)
        {
            // _animator.SetBool("hitRight", true);
            // _animator.SetTrigger("damage");
            // Debug.Log("Damage the Dummy");
        }
        
        public void Damage(float amount, Transform attackTransform)
        {
            // _animator.SetBool("hitRight", true);
            // _animator.SetTrigger("damage");
            // Debug.Log("Damage the Dummy");
        }
        
        public void Damage(float amount, IParryable parryable)
        {
            // _animator.SetBool("hitRight", true);
            // _animator.SetTrigger("damage");
            // Debug.Log("Damage the Dummy");
        }

        public void Knockback(Vector2 angle, float strength)
        {
            // _animator.SetBool("hitRight", direction != 1);
            // _animator.SetTrigger("damage");
            //
            // if (!canKnockback)
            // {
            //     return;
            // }
            //
            // angle.Normalize();
            // _workspace.Set(angle.x * strength * direction, angle.y * strength);
            // _rigidbody2D.velocity = _workspace;
        }
        
        public void Knockback(Vector2 angle, float strength, int direction)
        {
            _animator.SetBool("hitRight", direction != 1);
            _animator.SetTrigger("damage");
            
            if (!canKnockback)
            {
                return;
            }
            
            angle.Normalize();
            _workspace.Set(angle.x * strength * direction, angle.y * strength);
            _rigidbody2D.velocity = _workspace;
        }
        
        public void Knockback(Vector2 angle, float strength, int direction, IParryable parryable)
        {
            // _animator.SetBool("hitRight", direction != 1);
            // _animator.SetTrigger("damage");
            //
            // if (!canKnockback)
            // {
            //     return;
            // }
            //
            // angle.Normalize();
            // _workspace.Set(angle.x * strength * direction, angle.y * strength);
            // _rigidbody2D.velocity = _workspace;
        }
    }
}
