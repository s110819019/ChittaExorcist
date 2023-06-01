using System.Collections;
using System.Collections.Generic;
using ChittaExorcist.Common.Variables;
using ChittaExorcist.EventChannel;
using UnityEngine;
using UnityEngine.UI;

namespace ChittaExorcist
{
    public class PlayerStatsSliderUI : MonoBehaviour
    {
        [SerializeField] private FloatEventChannel listenFloatEvent;
        [SerializeField] private FloatReference maxValue;

        private Slider _slider;

        #region w/ On Value Change

        private void OnValueChange(float value)
        {
            _slider.value = value;
        }

        #endregion
        
        #region w/ Unity Callback Functions

        private void Awake()
        {
            TryGetComponent<Slider>(out _slider);
            if (_slider == null)
            {
                Debug.LogWarning("No Slider Component!");
                return;
            }

            _slider.maxValue = maxValue;
        }

        private void OnEnable()
        {
            // healthChannel.OnHealthChanged += OnHealthChanged;
            listenFloatEvent.AddListener(OnValueChange);
        }

        private void OnDisable()
        {
            // healthChannel.OnHealthChanged -= OnHealthChanged;
            listenFloatEvent.RemoveListener(OnValueChange);
        }    

        #endregion
    }
}
