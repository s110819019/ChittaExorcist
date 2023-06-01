using UnityEngine;

namespace ChittaExorcist.UISettings
{
    public class TipUIController : MonoBehaviour
    {
        // [SerializeField] private Sprite keyboardSprite;
        // [SerializeField] private Sprite gamepadSprite;

        private Sprite _keyboardSprite;
        private Sprite _gamepadSprite;

        public void InitSprite(Sprite keyboardSprite, Sprite gamepadSprite)
        {
            _keyboardSprite = keyboardSprite;
            _gamepadSprite = gamepadSprite;
        }
    }
}