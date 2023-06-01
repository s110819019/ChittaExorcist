using System;
using System.Collections;
using ChittaExorcist.Common.Variables;
using ChittaExorcist.PlayerSettings.FSM;
using UnityEngine;

namespace ChittaExorcist.CharacterCore
{
    public class PlayerMaterialManager : CoreComponent
    {
        [SerializeField] private FloatReference playerMana;
        [SerializeField] private FloatReference playerMaxMana;
        
        private readonly int _flashOpacity = Shader.PropertyToID("_FlashOpacity");
        private readonly int _mainOpacity = Shader.PropertyToID("_MainOpacity");
        private readonly int _useOutline = Shader.PropertyToID("_USEOUTLINE");

        private SpriteRenderer _spriteRenderer;
        private MaterialPropertyBlock _materialPropertyBlock;

        public Player.PlayerCharacter currentPlayer;

        #region w/ Variable

        #endregion
        
        #region w/ Events

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            _playerDamageReceiver.OnHit += OnStartHitFlash;
            _playerDamageReceiver.OnDamageOpacity += OnDamageOpacity;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            _playerDamageReceiver.OnHit -= OnStartHitFlash;
            _playerDamageReceiver.OnDamageOpacity -= OnDamageOpacity;
        }

        #endregion
        
        #region w/ Init

        public void InitializeSpriteRenderer(SpriteRenderer spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;

            // _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);

            _materialPropertyBlock ??= new MaterialPropertyBlock();
            
            _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);

            ResetAllMaterialVariableCheck();

            // ResetAllMaterialVariable();
            // ResetAllMaterialCoroutineVariable();
            // CheckOutline();
        }

        #endregion
        
        #region w/ Core Component
        
        private PlayerDamageReceiver _playerDamageReceiver;

        #endregion

        private void ResetAllMaterialVariable()
        {
            _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetFloat(_flashOpacity, 0.0f);
            _materialPropertyBlock.SetFloat(_mainOpacity, 1.0f);
            _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        private void ResetAllMaterialVariableCheck()
        {
            _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
            if (_hitFlashCoroutine == null)
            {
                _materialPropertyBlock.SetFloat(_flashOpacity, 0.0f);
            }
            else
            {
                _materialPropertyBlock.SetFloat(_flashOpacity, 0.6f);
            }

            if (_damageOpacityOnceCoroutine == null)
            {
                _materialPropertyBlock.SetFloat(_mainOpacity, 1.0f);
            }

            _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        private void ResetAllMaterialCoroutineVariable()
        {
            _isDamageOpacity = false;
            // hit flash
            if (_hitFlashCoroutine != null)
            {
                StopCoroutine(_hitFlashCoroutine);
            }
            _hitFlashCoroutine = null;

            // damage opacity
            if (_damageOpacityOnceCoroutine != null)
            {
                StopCoroutine(_damageOpacityOnceCoroutine);
            }
            _damageOpacityOnceCoroutine = null;
        }
        
        #region w/ Hit Flash

        private Coroutine _hitFlashCoroutine;

        private IEnumerator HitFlashCoroutine()
        {
            SetHitFlashOpacity(0.6f);
            
            yield return new WaitForSeconds(0.125f);

            SetHitFlashOpacity(0.0f);
            
            _hitFlashCoroutine = null;
        }
        
        private void SetHitFlashOpacity(float opacityValue)
        {

            _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetFloat(_flashOpacity, opacityValue);

            _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        private void OnStartHitFlash()
        {
            if (_hitFlashCoroutine != null)
            {
                StopCoroutine(_hitFlashCoroutine);
            }

            _hitFlashCoroutine = StartCoroutine(HitFlashCoroutine());
        }

        #endregion

        #region w/ Damage Opacity
        
        private Coroutine _damageOpacityOnceCoroutine;
        private bool _isDamageOpacity;

        private IEnumerator DamageOpacityOnceCoroutine()
        {
            SetDamageOpacity(0.2f);

            yield return new WaitForSeconds(0.125f);
            
            SetDamageOpacity(1.0f);
            
            yield return new WaitForSeconds(0.125f);

            _damageOpacityOnceCoroutine = null;
        }

        private void StartDamageOpacityOnce()
        {
            if (_damageOpacityOnceCoroutine == null)
            {
                _damageOpacityOnceCoroutine = StartCoroutine(DamageOpacityOnceCoroutine());
            }
        }
        
        private void SetDamageOpacity(float opacityValue)
        {
            _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetFloat(_mainOpacity, opacityValue);
            _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        private void OnDamageOpacity(bool value)
        {
            _isDamageOpacity = value;
            if (!value)
            {
                if (_damageOpacityOnceCoroutine != null)
                {
                    StopCoroutine(_damageOpacityOnceCoroutine);
                    _damageOpacityOnceCoroutine = null;
                }
                SetDamageOpacity(1.0f);
            }
        }

        private void CheckDamageOpacity()
        {
            if (_isDamageOpacity)
            {
                StartDamageOpacityOnce();
            }
        }

        #endregion

        #region w/ Outline
        
        private void SetOutline(bool outlineBool)
        {
            // _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
            if (outlineBool)
            {
                _spriteRenderer.material.EnableKeyword("_USEOUTLINE");
            }
            else
            {
                _spriteRenderer.material.DisableKeyword("_USEOUTLINE");
            }
            // _materialPropertyBlock.SetFloat(_useOutline, outlineBool ? 1 : 0);
            // _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        private void CheckOutline()
        {
            switch (currentPlayer)
            {
                case Player.PlayerCharacter.ChuXiao:
                    if (playerMana.Value == playerMaxMana.Value)
                    {
                        SetOutline(true);
                    }
                    else
                    {
                        SetOutline(false);
                    }
                    break;
                case Player.PlayerCharacter.ShaoYue:
                    // SetOutline(true);
                    break;
            }
        }

        #endregion

        #region w/ Unity Callback Functions

        protected override void Awake()
        {
            base.Awake();
            Core.GetCoreComponent(out _playerDamageReceiver);
        }

        protected override void Start()
        {
            base.Start();
        }

        private void Update()
        {
            CheckDamageOpacity();
            CheckOutline();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StopAllCoroutines();
            ResetAllMaterialCoroutineVariable();
            ResetAllMaterialVariable();
        }

        #endregion
    }
}