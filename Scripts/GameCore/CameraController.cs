using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace ChittaExorcist.GameCore
{
    // public class CameraController : MonoBehaviour
    // {
    //     #region w/ Camera Controller
    //
    //     public static CameraController Instance;
    //
    //     #endregion
    //
    //     #region w/ Camera
    //
    //     // main camera
    //     private Camera _camera;
    //
    //     // cinemachine brain
    //     private CinemachineBrain _cineBrain;
    //
    //     // virtual camera
    //     private CinemachineVirtualCamera _currentVirtualCamera;
    //
    //     private CinemachineVirtualCamera CurrentVirtualCamera
    //     {
    //         get
    //         {
    //             if (_cineBrain.ActiveVirtualCamera as CinemachineVirtualCamera == null)
    //             {
    //                 Debug.LogWarning("Not Found ActiveVirtualCamera !");
    //             }
    //             return _cineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
    //         }
    //         set => _currentVirtualCamera = value;
    //     }
    //
    //     #endregion
    //
    //     #region w/ Camera Shake
    //
    //     private CinemachineBasicMultiChannelPerlin _cineBmcPerlin;
    //     
    //     // private float _cameraShakeTimer;
    //
    //     private Tween _cameraShakeTween;
    //     
    //     #endregion
    //
    //     #region w/ Camera Transposer
    //
    //     private CinemachineFramingTransposer _cineFramingTransposer;
    //     private CinemachineCameraOffset _cineCameraOffset;
    //
    //     private float _offsetTime = 1.0f;
    //
    //     private bool _isOffset;
    //     
    //     private Tween _cameraOffsetTween;
    //     
    //
    //     #endregion
    //
    //     #region w/ Unity Callback Functions
    //
    //     private void Awake()
    //     {
    //         Instance = this;
    //     }
    //
    //     [ContextMenu("Test Check Current Active Cinemachine")]
    //     private void TestCheckCurrentActiveCinemachine()
    //     {
    //         Debug.Log("Current Active VCam : " + CurrentVirtualCamera);
    //     }
    //     
    //     private void Start()
    //     {
    //         _camera = Camera.main;
    //         if (_camera == null)
    //         {
    //             Debug.Log("Not Found Main Camera");
    //         }
    //         else
    //         {
    //             _cineBrain = _camera.GetComponent<CinemachineBrain>();
    //         }
    //         // Debug.Log("Cine Brain : " + _cineBrain);
    //         
    //         // Debug.Log("Cine Brain : " + _cineBrain.ActiveVirtualCamera);
    //         // _currentVirtualCamera = _cineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
    //         // _currentVirtualCamera = _cineBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
    //         // Debug.Log("Cine Cam : " + _currentVirtualCamera);
    //     }
    //
    //     private void UpdateVirtualCameraComponents()
    //     {
    //         _cineBmcPerlin = CurrentVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    //     }
    //
    //     public void ShakeCamera(float strength, float frequency, float duration = 0.2f, Ease ease = Ease.InOutQuart)
    //     {
    //         UpdateVirtualCameraComponents();
    //         StopShakeCamera();
    //         
    //         _cineBmcPerlin.m_AmplitudeGain = strength;
    //         _cineBmcPerlin.m_FrequencyGain = frequency;
    //         _cameraShakeTween = DOTween.To(() => _cineBmcPerlin.m_AmplitudeGain,
    //                 (value) => _cineBmcPerlin.m_AmplitudeGain = value, 0, duration)
    //             .SetAutoKill(true)
    //             .SetUpdate(true)
    //             .SetEase(ease)
    //             .OnKill(() => _cameraShakeTween = null);
    //     }
    //
    //     public void StopShakeCamera()
    //     {
    //         _cineBmcPerlin.m_AmplitudeGain = 0;
    //         _cineBmcPerlin.m_FrequencyGain = 0;
    //         if (_cameraShakeTween == null) return;
    //         if (_cameraShakeTween.IsPlaying())
    //         {
    //             _cameraShakeTween.Kill();
    //         }
    //         _cameraShakeTween = null;
    //     }
    //
    //     private void Update()
    //     {
    //         // if (_cameraShakeTimer > 0.0f)
    //         // {
    //         //     _cameraShakeTimer -= Time.deltaTime;
    //         //
    //         //     if (_cameraShakeTimer <= 0.0f)
    //         //     {
    //         //         StopShakeCamera();
    //         //     }
    //         // }
    //     }
    //
    //     public void MoveTransposer(int yInput)
    //     {
    //         _cineFramingTransposer = CurrentVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    //         _cineFramingTransposer.m_TrackedObjectOffset =
    //             new Vector3(_cineFramingTransposer.m_TrackedObjectOffset.x, 10 * yInput, _cineFramingTransposer.m_TrackedObjectOffset.z);
    //     }
    //     
    //     public void MoveTransposerOffset(int yInput)
    //     {
    //         if (_isOffset)
    //         {
    //             return;
    //         }
    //         else
    //         {
    //             _isOffset = true;
    //         }
    //         
    //         _cineCameraOffset = CurrentVirtualCamera.GetComponent<CinemachineCameraOffset>();
    //         
    //         _cameraOffsetTween?.Kill();
    //
    //         _cameraOffsetTween = DOTween.To(() => _cineCameraOffset.m_Offset,
    //                 (value) => _cineCameraOffset.m_Offset = value, new Vector3(0, 10, 0), _offsetTime)
    //             .SetEase(Ease.Linear)
    //             .SetUpdate(true).OnComplete(() => _isOffset = false);
    //
    //         // _cineCameraOffset.m_Offset =
    //         //     new Vector3(_cineFramingTransposer.m_TrackedObjectOffset.x, 10 * yInput, _cineFramingTransposer.m_TrackedObjectOffset.z);
    //     }
    //
    //     #endregion
    // }
}