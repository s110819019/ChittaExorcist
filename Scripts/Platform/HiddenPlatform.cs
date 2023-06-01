using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ChittaExorcist.PlatformSettings
{
    public class HiddenPlatform : MonoBehaviour
    {
        private Tilemap _tilemap;
        private float _animationTime = 0.5f;
        private Tween _tween;

        #region w/ Unity Callback Functions

        private void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
        }

        #endregion

        #region w/ Trigger

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _tween?.Kill();
            _tween = DOTween
                .To(() => _tilemap.color, (value) => _tilemap.color = value, new Color(1, 1, 1, 0), _animationTime)
                .SetEase(Ease.Linear).SetUpdate(true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _tween?.Kill();
            _tween = DOTween
                .To(() => _tilemap.color, (value) => _tilemap.color = value, new Color(1, 1, 1, 1), _animationTime)
                .SetEase(Ease.Linear).SetUpdate(true);
        }

        #endregion

        private void OnDestroy()
        {
            _tween?.Kill();
        }
    }
}
