using System;
using ChittaExorcist.GameCore;
using DG.Tweening;
using UnityEngine;

namespace ChittaExorcist.InteractableObject
{
    public class SoulObject : MonoBehaviour
    {
        [Header("Fade in")]
        [SerializeField] private float fadeTime = 1.0f;

        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private Collider2D _collider;

        private void Awake()
        {
            TryGetComponent(out _spriteRenderer);
            TryGetComponent(out _animator);
            TryGetComponent(out _collider);
        }

        private void OnEnable()
        {
            _collider.enabled = false;
            _animator.Play("I_Soul_Idle");
            _spriteRenderer.DOKill();
            _spriteRenderer.DOFade(1, fadeTime)
                .SetEase(Ease.InCirc)
                .SetUpdate(true)
            // .OnComplete(() => gameObject.SetActive(false));
            .OnComplete(() => _collider.enabled = true);
        }

        private void OnDisable()
        {
            var temp = _spriteRenderer.color;
            temp.a = 0.0f;
        }

        public void AnimationFinish()
        {
            _collider.enabled = false;
            _spriteRenderer.DOKill();
            _spriteRenderer.DOFade(0, 0);
            ObjectPoolManager.Instance.ReturnObject(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _animator.Play("I_Soul_End");
            }
        }
    }
}