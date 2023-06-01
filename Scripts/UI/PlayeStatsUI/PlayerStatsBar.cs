using System.Collections;
using System.Collections.Generic;
using ChittaExorcist.Common.Variables;
using ChittaExorcist.EventChannel;
using UnityEngine;
using UnityEngine.UI;

namespace ChittaExorcist
{
    public class PlayerStatsBar : MonoBehaviour
    {
        [SerializeField] private FloatEventChannel onPlayerStatChange;
        [SerializeField] private IntEventChannel onPlayerSwitch;

        [SerializeField] private Image frontFill;
        [SerializeField] private Image backFill;

        [SerializeField] private Color firstColor;
        [SerializeField] private Color secondColor;
        
        [SerializeField] private FloatReference maxValue;

        [SerializeField] private float fillSpeed = 0.2f;
        [SerializeField] private float fillDelay = 0.2f;

        private bool _isDelayFill;
        private WaitForSeconds _waitForDelayFill;
        private Coroutine _bufferedFillingCoroutine;
        
        private float _targetFillAmount;
        private float _currentFillAmount;
        private float _previousFillAmount;
        private float _temp;

        private IEnumerator BufferedFillingCoroutine(Image image)
        {
            if (_isDelayFill)
            {
                yield return _waitForDelayFill;
            }

            _previousFillAmount = _currentFillAmount;
            
            _temp = 0.0f;
            
            while (_temp < 1.0f)
            {
                _temp += Time.deltaTime * fillSpeed;
                _currentFillAmount = Mathf.Lerp(_previousFillAmount, _targetFillAmount, _temp);
                image.fillAmount = _currentFillAmount;

                yield return null;
            }
        }
        
        #region w/ On Value Change

        private void OnValueChange(float value)
        {
            _targetFillAmount = value / maxValue;

            if (_bufferedFillingCoroutine != null)
            {
                StopCoroutine(_bufferedFillingCoroutine);
            }
            
            // decrease
            if (_currentFillAmount > _targetFillAmount)
            {
                frontFill.fillAmount = _targetFillAmount;
                StartCoroutine(BufferedFillingCoroutine(backFill));
                return;
            }
            // increase
            if (_currentFillAmount < _targetFillAmount)
            {
                backFill.fillAmount = _targetFillAmount;
                StartCoroutine(BufferedFillingCoroutine(frontFill));
            }
            
        }

        #endregion
        
        #region w/ Charge Health Bar Color
        
        private void OnPlayerSwitch(int value)
        {
            if (value == 0)
            {
                frontFill.color = firstColor;
            }
            else if (value == 1)
            {
                frontFill.color = secondColor;
            }
            else
            {
                Debug.LogWarning("Set Wrong Number when on player switch event !");
            }
        }
        
        #endregion
        
        #region w/ Unity Callback Functions

        private void Awake()
        {
            _waitForDelayFill = new WaitForSeconds(fillDelay);
            _currentFillAmount = _targetFillAmount = 1;
        }

        private void OnEnable()
        {
            // healthChannel.OnHealthChanged += OnHealthChanged;
            onPlayerStatChange.AddListener(OnValueChange);
            onPlayerSwitch.AddListener(OnPlayerSwitch);
        }

        private void OnDisable()
        {
            // healthChannel.OnHealthChanged -= OnHealthChanged;
            StopAllCoroutines();
            onPlayerStatChange.RemoveListener(OnValueChange);
            onPlayerSwitch.RemoveListener(OnPlayerSwitch);
        }    

        #endregion
    }
}
