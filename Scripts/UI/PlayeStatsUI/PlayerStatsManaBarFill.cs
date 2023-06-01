using ChittaExorcist.EventChannel;
using UnityEngine;
using UnityEngine.UI;

namespace ChittaExorcist
{
    public class PlayerStatsManaBarFill : MonoBehaviour
    {
        [SerializeField] private IntEventChannel onPlayerSwitch;

        [SerializeField] private Color brightColor;

        private Image _manaBarImage;
        
        #region w/ Charge Health Bar Color

        private void OnPlayerSwitch(int value)
        {
            if (value == 0)
            {
                _manaBarImage.color = Color.white;
            }
            else if (value == 1)
            {
                _manaBarImage.color = brightColor;
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
            TryGetComponent<Image>(out _manaBarImage);
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