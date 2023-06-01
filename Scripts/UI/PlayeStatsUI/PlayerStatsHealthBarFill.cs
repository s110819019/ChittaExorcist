using ChittaExorcist.EventChannel;
using UnityEngine;
using UnityEngine.UI;

namespace ChittaExorcist
{
    public class PlayerStatsHealthBarFill : MonoBehaviour
    {
        [SerializeField] private IntEventChannel onPlayerSwitch;

        [SerializeField] private Color darkColor;

        private Image _healthBarImage;
        
        #region w/ Charge Health Bar Color

        private void OnPlayerSwitch(int value)
        {
            if (value == 0)
            {
                _healthBarImage.color = Color.white;
            }
            else if (value == 1)
            {
                _healthBarImage.color = darkColor;
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
            TryGetComponent<Image>(out _healthBarImage);
        }

        private void OnEnable()
        {
            onPlayerSwitch.AddListener(OnPlayerSwitch);
        }

        private void OnDisable()
        {
            onPlayerSwitch.RemoveListener(OnPlayerSwitch);
        }    

        #endregion
    }
}