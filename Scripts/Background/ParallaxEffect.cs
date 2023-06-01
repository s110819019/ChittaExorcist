using System;
using ChittaExorcist.GameCore;
using UnityEngine;

namespace ChittaExorcist.BackgroundSettings
{
    public class ParallaxEffect : MonoBehaviour
    {
        #region w/ Druid

        // 相機
        [SerializeField] private GameObject targetCamera;

        // 視差係數
        [SerializeField, Range(-1.0f, 1.0f)] private float multiplier;
        // 只用水平視差(垂直不做移動)
        [SerializeField] private bool horizontalOnly = true;

        [Header("Calculate Infinite")]
        [SerializeField] private bool calculateInfiniteHorizontalPosition = true;
        [SerializeField] private bool calculateInfiniteVerticalPosition;
    

        private Vector3 _startPosition;
        private Vector3 _startCameraPosition;

        private void CalculateStartPosition()
        {
            float distanceX = (targetCamera.transform.position.x - transform.position.x) * multiplier;
            float distanceY = (targetCamera.transform.position.y - transform.position.y) * multiplier;
        
            Vector3 temp = new Vector3(_startPosition.x, _startPosition.y);

            if (calculateInfiniteHorizontalPosition)
            {
                temp.x = transform.position.x + distanceX;
            }

            if (calculateInfiniteVerticalPosition)
            {
                temp.y = transform.position.y + distanceY;
            }

            _startPosition = temp;
        }
    
        private void Start()
        {
            if (targetCamera == null)
            {
                targetCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
            
            if (targetCamera == null)
            {
                targetCamera = CameraManager.Instance.MainCamera.gameObject;
            }

            _startPosition = transform.position;
            _startCameraPosition = targetCamera.transform.position;
            // _sceneStartPosition = sceneStartPoint.transform.position;


            CalculateStartPosition();
        }

        private void LateUpdate()
        {
            Vector3 position = _startPosition;

            if (horizontalOnly)
            {
                position.x += multiplier * (targetCamera.transform.position.x - _startCameraPosition.x);
            }
            else
            {
                position += multiplier * (targetCamera.transform.position - _startCameraPosition);
            }

            transform.position = position;
        }

        #endregion
    }
}