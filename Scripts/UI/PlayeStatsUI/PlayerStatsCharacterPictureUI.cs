using ChittaExorcist.EventChannel;
using ChittaExorcist.PlayerEffectSettings;
using UnityEngine;
using UnityEngine.UI;

namespace ChittaExorcist
{
    public class PlayerStatsCharacterPictureUI : MonoBehaviour
    {
        [SerializeField] private IntEventChannel onPlayerSwitch;
        [SerializeField] private Image[] characterPicture;
        [SerializeField] private PlayerEffect playerSwitchEffect;

        #region w/ Handle Player Switch

        private void OnPlayerSwitch(int value)
        {
            switch (value)
            {
                case 0:
                    characterPicture[0].gameObject.SetActive(true);
                    characterPicture[1].gameObject.SetActive(false);
                    break;
                case 1:
                    characterPicture[0].gameObject.SetActive(false);
                    characterPicture[1].gameObject.SetActive(true);
                    break;
                default:
                    Debug.Log(" Set a wrong current player to CharacterPictureUI !");
                    break;
            }
            playerSwitchEffect.PlayAnimationFromZero("Start");
        }

        #endregion
        
        #region w/ Unity Callback Functions

        private void Awake()
        {
            characterPicture[0].gameObject.SetActive(true);
            characterPicture[1].gameObject.SetActive(false);
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