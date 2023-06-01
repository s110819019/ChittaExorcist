using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ChittaExorcist.InteractableObject
{
    public class ShaderFoliageObject : MonoBehaviour
    {
        private readonly int _foliageSpeedID = Shader.PropertyToID("_FoliageSpeed");
        private readonly int _foliageStrengthID = Shader.PropertyToID("_FoliageStrength");

        private bool _isChanging;
        private float _waitForSecond = 1.0f;

        private float _foliageSpeed;
        private float _foliageStrength;

        public Vector2 foliageSpeed;
        public Vector2 foliageStrength;

        private int _direction;
        
        
        private SpriteRenderer _spriteRenderer;
        private MaterialPropertyBlock _materialPropertyBlock;

        private Coroutine _attenuationCoroutine;

        private IEnumerator Attenuation()
        {
            _isChanging = true;

            yield return new WaitForSeconds(_waitForSecond);

            DOTween.To(() => _foliageSpeed, value => _foliageSpeed = value, foliageSpeed.x, 1.0f);
            DOTween.To(() => _foliageStrength, value => _foliageStrength = value, foliageStrength.x * _direction, 1.0f);
            
            yield return new WaitForSeconds(_waitForSecond);
            
            _attenuationCoroutine = null;
            _isChanging = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            // Debug.Log("Player Enter Grass");

            _direction = transform.position.x > other.transform.position.x ? 1 : -1;
            DOTween.To(() => _foliageSpeed, value => _foliageSpeed = value, foliageSpeed.y, 0.3f);
            DOTween.To(() => _foliageStrength, value => _foliageStrength = value, foliageStrength.y * _direction, 0.3f);

            if (_attenuationCoroutine != null)
            {
                StopCoroutine(_attenuationCoroutine);
            }

            _attenuationCoroutine = StartCoroutine(Attenuation());
            // StartCoroutine(Attenuation());
        }

        private void Start()
        {
            TryGetComponent(out _spriteRenderer);
            _materialPropertyBlock ??= new MaterialPropertyBlock();
            if (_spriteRenderer != null)
            {
                _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
            }
            else
            {
                Debug.LogWarning("No SpriteRenderer On Foliage Shader Object");
            }
        }

        private void Update()
        {
            if (!_isChanging) return;
            
            // _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
            
            // _materialPropertyBlock.Clear();
            // _materialPropertyBlock.SetFloat(_foliageSpeedID, foliageSpeed.y);
            _materialPropertyBlock.SetFloat(_foliageSpeedID, _foliageSpeed);
            _materialPropertyBlock.SetFloat(_foliageStrengthID, _foliageStrength);

            _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
        }
    }
}
