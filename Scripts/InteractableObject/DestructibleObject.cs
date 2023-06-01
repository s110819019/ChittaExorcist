using System;
using System.Collections;
using System.Collections.Generic;
using ChittaExorcist.Common;
using ChittaExorcist.Common.Interfaces;
using ChittaExorcist.Structs;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace ChittaExorcist.InteractableObject
{
    public class DestructibleObject : MonoBehaviour, IDestructible
    {
        [Header("Object Health")]
        [SerializeField] private int health = 2;
        
        [Header("Debris")]
        [SerializeField] private ParticleSystem objectParticle;
        
        [Header("Do Punch")]
        [SerializeField] private PunchDetails punchDetails;
        
        [Header("Fade out")]
        [SerializeField] private float fadeTime;

        #region w/ Components

        private SpriteRenderer _spriteRenderer;

        #endregion

        #region w/ Variables

        private bool _isDead;
        private bool _isFade;

        #endregion
        
        public int Health
        {
            get => health;
            private set => health = Mathf.Clamp(value, 0, 10);
        }

        public void Damage(int direction)
        {
            if (_isDead)
            {
                return;
            }
            
            Shake(direction);
            Health--;
            if (Health == 0)
            {
                Dead();
            }
        }

        private void Shake(int direction)
        {
            // Debug.Log("Shake Obj");
            _spriteRenderer.DOKill();
            _spriteRenderer.transform.DOKill();
            // _spriteRenderer.transform.DOShakePosition(0.2f, 0.5f, 1, 1, false, true)
            //     .SetUpdate(false);
            _spriteRenderer.transform.DOPunchPosition(
                punchDetails.punchDirection * (direction >= 0 ? 1 : -1),
                punchDetails.punchDuration,
                punchDetails.punchVibrato, punchDetails.punchElasticity);
        }

        private void Dead()
        {
            objectParticle.Play();
            _isDead = true;
            
            _spriteRenderer.DOKill();
            _spriteRenderer.DOFade(0, fadeTime)
                .SetEase(Ease.InCirc)
                .SetUpdate(true)
                // .OnComplete(() => gameObject.SetActive(false));
                .OnComplete(() => _isFade = true);

            // _objectPool.GetObject(objectParticle);
        }

        #region w/ Unity Callback Functions

        private void Awake()
        {
            TryGetComponent(out _spriteRenderer);
            if (_spriteRenderer == null)
            {
                Debug.LogWarning(" No SpriteRenderer on this Destructible Object ");
            }
        }
        
        private void Update()
        {
            if (!_isDead)
            {
                return;
            }

            if (!objectParticle.IsAlive() && _isFade)
            {
                gameObject.SetActive(false);
            }
        }

        #endregion
    }
}