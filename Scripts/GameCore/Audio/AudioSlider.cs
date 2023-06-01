using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace ChittaExorcist.GameCore.AudioSettings
{
    public class AudioSlider : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private AudioMixMode mixMode = AudioMixMode.LogarithmicMixerVolume;
        [SerializeField] private AudioMixController mixController;

        private Slider _slider;
        
        private string _targetVolume;
        
        public void OnChangeSlider(float value)
        {
            if (valueText)
            {
                valueText.SetText($"{value}");
            }

            var tempValue = value / 10;

            PlayerPrefs.SetFloat(_targetVolume, value);
            
            switch (mixMode)
            {
                case AudioMixMode.LinearMixerVolume:
                    mixer.SetFloat(_targetVolume, (-80 + tempValue * 100));
                    break;
                case AudioMixMode.LogarithmicMixerVolume:
                    mixer.SetFloat(_targetVolume, value == 0 ? -80 : Mathf.Log10(tempValue) * 20);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnEnable()
        {
            switch (_targetVolume)
            {
                case "MainVolume":
                    if (PlayerPrefs.HasKey("MainVolume"))
                    {
                        OnChangeSlider(PlayerPrefs.GetFloat("MainVolume"));
                        _slider.value = PlayerPrefs.GetFloat("MainVolume");
                    }
                    break;
                case "STVolume":
                    if (PlayerPrefs.HasKey("STVolume"))
                    {
                        OnChangeSlider(PlayerPrefs.GetFloat("STVolume"));
                        _slider.value = PlayerPrefs.GetFloat("STVolume");
                    }
                    break;
                case "SFXVolume":
                    if (PlayerPrefs.HasKey("SFXVolume"))
                    {
                        OnChangeSlider(PlayerPrefs.GetFloat("SFXVolume"));
                        _slider.value = PlayerPrefs.GetFloat("SFXVolume");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            
        }

        private void Awake()
        {
            switch (mixController)
            {
                case AudioMixController.Main:
                    _targetVolume = "MainVolume";
                    break;
                case AudioMixController.ST:
                    _targetVolume = "STVolume";
                    break;
                case AudioMixController.SFX:
                    _targetVolume = "SFXVolume";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            TryGetComponent(out _slider);
        }

        private enum AudioMixMode
        {
            LinearMixerVolume,
            LogarithmicMixerVolume
        }

        private enum AudioMixController
        {
            Main,
            ST,
            SFX
        }
    }
}