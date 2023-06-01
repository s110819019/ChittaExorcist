using System;
using UnityEngine;

namespace ChittaExorcist.InteractableObject
{
    public class PlantWave : MonoBehaviour
    {
        private readonly int _xOffsetShader = Shader.PropertyToID("_xOffset");
        private readonly int _timeStrengthShader = Shader.PropertyToID("_TimeStrength");
        private readonly string _useTimeOffset = "_USETIME";
        
        public float maxXOffset = 0.5f;
        public float waveSpeed = 0.15f;
        public float timeStrength = 2.0f;

        private float _waveToAngle;
        private float _xOffset = 0.0f;
        private float _timeStrength = 0.0f;
        // private Material _material;
        private SpriteRenderer _spriteRenderer;
        private MaterialPropertyBlock _materialPropertyBlock;

        private Rigidbody2D _rigidbody2D;
        
        private bool _playerIn;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerIn = true;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (_rigidbody2D == null)
                {
                    other.TryGetComponent(out _rigidbody2D);
                }
                if (_rigidbody2D == null)
                {
                    Debug.LogWarning("PlantWave Script can not get player rigidbody2D");
                    return;
                }
                
                float speedRate = _rigidbody2D.velocity.x / 8.0f;
                _waveToAngle = speedRate * maxXOffset;
                
                // if (Mathf.Abs(_rigidbody2D.velocity.x) > 10.0f)
                // {
                //     float speedRate = _rigidbody2D.velocity.x / 8.0f;
                //     _waveToAngle = speedRate * maxXOffset;
                // }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // SetTimeOffset(true);
                _playerIn = false;
                _waveToAngle = 0.0f;
                _timeStrength = 0.0f;
            }
        }

        private void SetTimeOffset(bool value)
        {
            if (value)
            {
                _spriteRenderer.material.EnableKeyword(_useTimeOffset);
            }
            else
            {
                _spriteRenderer.material.DisableKeyword(_useTimeOffset);
            }
        }

        private void SetXOffset()
        {
            _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetFloat(_xOffsetShader, _xOffset);
            _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        private void SetTimeStrength()
        {
            _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetFloat(_timeStrengthShader, _timeStrength);
            _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
        }
        
        #region w/ Unity Functions

        private void Start()
        {
            TryGetComponent(out _spriteRenderer);
            if (_spriteRenderer == null)
            {
                Debug.LogWarning("PlantWave Script can not get spriteRenderer");
                return;
            }
            _materialPropertyBlock ??= new MaterialPropertyBlock();
            _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
            _timeStrength = timeStrength;
            SetTimeOffset(true);
        }

        private void Update()
        {
            if (_xOffset != _waveToAngle)
            {
                SetTimeOffset(false);

                _xOffset = Mathf.Lerp(_xOffset, _waveToAngle, waveSpeed);
                if (MathF.Abs((_xOffset - _waveToAngle)) <= 0.01f)
                {
                    // _xOffset = _waveToAngle = 0.0f;
                    _xOffset = _waveToAngle;
                }
                SetXOffset();
            }
            else
            {
                if (!_playerIn)
                {
                    // _xOffset = _waveToAngle = 0.0f;
                    SetTimeOffset(true);                    
                }
            }

            if (!_playerIn && _xOffset == 0.0f && _timeStrength != timeStrength)
            {
                _timeStrength = Mathf.Lerp(_timeStrength, timeStrength, waveSpeed);
                if (MathF.Abs((_timeStrength - timeStrength)) <= 0.01f)
                {
                    _timeStrength = timeStrength;
                }
                SetTimeStrength();
            }
        }

        #endregion
    }
}