using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace ChittaExorcist.CharacterCore
{
    public class MaterialManager : CoreComponent
    {
        private readonly int _flashOpacity = Shader.PropertyToID("_FlashOpacity");
        // private readonly int _dissolveScale = Shader.PropertyToID("_dissolveScale");
        private readonly int _dissolveFade = Shader.PropertyToID("_DissolveFade");

        private SpriteRenderer _spriteRenderer;
        private MaterialPropertyBlock _materialPropertyBlock;

        public event Action OnDeathOver;

        #region w/ Variable

        private float _dissolveCurrentFadeValue;
        private float _dissolveDuration = 2.0f;
        private float _dissolveDefaultFadeValue = 1.0f;

        private bool _hasDeath;
        
        #endregion
        
        #region w/ Events

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            _damageReceiver.Comp.OnHit += OnStartHitFlash;
            _stats.Comp.OnHealthZero += OnStartDeathDissolve;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            _damageReceiver.Comp.OnHit -= OnStartHitFlash;
            _stats.Comp.OnHealthZero -= OnStartDeathDissolve;
        }

        #endregion
        
        #region w/ Init

        public void InitializeSpriteRenderer(SpriteRenderer spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;

            // _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);

            _materialPropertyBlock ??= new MaterialPropertyBlock();
            
            _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
        }        

        #endregion
        
        #region w/ Core Component

        private CoreComp<DamageReceiver> _damageReceiver;
        private CoreComp<Stats> _stats;

        #endregion
        
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
            if (_hasDeath)
            {
                return;
            }
            
            if (_hitFlashCoroutine != null)
            {
                StopCoroutine(_hitFlashCoroutine);
            }

            _hitFlashCoroutine = StartCoroutine(HitFlashCoroutine());
        }
        
        #endregion

        #region w/ Death Dissolve

        private Tween _deathDissolveTween;

        private void DeathDissolveTween()
        {
            _deathDissolveTween?.Kill();

            _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);

            _deathDissolveTween = DOTween
                .To(() => _dissolveCurrentFadeValue, value => _dissolveCurrentFadeValue = value, 0, _dissolveDuration)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .OnUpdate(() =>
                {
                    // Debug.Log(_dissolveCurrentFadeValue);
                    _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
                    _materialPropertyBlock.SetFloat(_dissolveFade, _dissolveCurrentFadeValue);
                    _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
                })
                .OnComplete(() => OnDeathOver?.Invoke());
        }

        private void OnStartDeathDissolve()
        {
            // Debug.Log("Start Death Dissolve");
            if (_hasDeath)
            {
                return;
            }

            _hasDeath = true;
            _dissolveCurrentFadeValue = _dissolveDefaultFadeValue;
            DeathDissolveTween();
        }

        #endregion
        
        #region w/ Unity Callback Functions

        protected override void Awake()
        {
            base.Awake();
            _damageReceiver = new CoreComp<DamageReceiver>(Core);
            _stats = new CoreComp<Stats>(Core);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StopAllCoroutines();
        }

        #endregion
    }
}