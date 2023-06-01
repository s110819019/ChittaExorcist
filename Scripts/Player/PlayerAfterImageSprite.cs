using UnityEngine;

namespace ChittaExorcist.PlayerSettings
{
    public class PlayerAfterImageSprite : MonoBehaviour
    {
        [SerializeField] private float activeTime = 0.1f;   // 啟用時間
        [SerializeField] private float alphaSet = 0.8f;     // 起始透明度
        [SerializeField] private float alphaDecay = 10.0f;  // 透明度遞減程度
    
        private float _timeActivated;   // 開始時間
        private float _alpha;           // 透明度
        private Transform _player;      // 玩家位置

        private SpriteRenderer _spriteRenderer;         // 物件的 sprite renderer
        private SpriteRenderer _playerSpriteRenderer;   // 角色的 sprite renderer

        private Color _color; // 顏色

        #region w/ Unity Callback Functions
    
        private void OnEnable()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        
            // 避免使用 Find
            // _player = GameObject.FindGameObjectWithTag("Player").transform;
            _player = PlayerAfterImagePool.Instance.Player.transform;
        
            _playerSpriteRenderer = _player.GetComponent<SpriteRenderer>();

            _alpha = alphaSet;
            _spriteRenderer.sprite = _playerSpriteRenderer.sprite;
        
            transform.position = _player.parent.position;
            transform.rotation = _player.parent.rotation;
        
            _timeActivated = Time.time;
        }

        private void Update()
        {
            _alpha -= alphaDecay * Time.deltaTime; // 透明度隨時間遞減
            _color = new Color(1.0f, 1.0f, 1.0f, _alpha);
            _spriteRenderer.color = _color;

            if (Time.time >= _timeActivated + activeTime)
            {
                // 加回 player after image 物件池裡面
                PlayerAfterImagePool.Instance.AddToPool(gameObject);
            }
        }

        #endregion
    }
}