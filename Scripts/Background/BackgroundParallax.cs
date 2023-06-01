using ChittaExorcist.GameCore;
using UnityEngine;

namespace ChittaExorcist.BackgroundSettings
{
    public class BackgroundParallax : MonoBehaviour
    {
        [Header("Main Camera")]
        public Camera cam;
        // [Header("Target")]
        // public Transform subject;
        [Header("Constrain Y Position for Background")]
        public bool constrainYPosition;

        // Start
        private Vector2 _startPosition;
        // Start Z
        private float _startZPosition;

        // 與 相機 的距離
        private Vector2 Travel => (Vector2) cam.transform.position - _startPosition;
    
        // 與 玩家 的 z 距離 (this - target)
        // private float DistanceFromSubject => transform.position.z - subject.position.z;
        private float DistanceFromSubject => transform.position.z;
    
        // 與 玩家 的 z 距離為負 => cam Z + cam Near (0.3)
        // 與 玩家 的 z 距離為正 => cam Z + cam Far  (100)
        private float ClippingPlane => ((cam.transform.position.z) + (DistanceFromSubject > 0 ? cam.farClipPlane : cam.nearClipPlane));
    
        // 與 玩家 的 z 距離為負 => |與 玩家 的 z 距離 / 0.3 | => 移動快
        // 與 玩家 的 z 距離為正 => |與 玩家 的 z 距離 / 100 | => 移動慢
        private float ParallaxFactor => (Mathf.Abs(DistanceFromSubject) / ClippingPlane);

    
        public void Start()
        {
            _startPosition = transform.position;
            _startZPosition = transform.position.z;

            if (cam == null)
            {
                cam = CameraManager.Instance.MainCamera;
            }
        }

        public void LateUpdate()
        {
            Vector2 newPos = _startPosition + Travel * ParallaxFactor;

            if (constrainYPosition)
            {
                Vector2 oldPos = _startPosition + Travel;
                transform.position = new Vector3(newPos.x, oldPos.y, _startZPosition);
            }
            else
            {
                transform.position = new Vector3(newPos.x, newPos.y, _startZPosition);
            }
        }
    }
}