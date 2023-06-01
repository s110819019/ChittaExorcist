using System;
using ChittaExorcist.Common.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace ChittaExorcist
{
    public class PlayerStatsSliderTextUITest : MonoBehaviour
    {
        [SerializeField] private String sliderTypeName;
        [SerializeField] private FloatReference currentValue;
        [SerializeField] private FloatReference maxValue;

        private Text _text;
        
        #region w/ Unity Callback Function

        private void Awake()
        {
            TryGetComponent<Text>(out _text);
        }

        private void Update()
        {
            _text.text = $"{sliderTypeName} : {currentValue.Value} / {maxValue.Value}";
        }

        #endregion
    }
}