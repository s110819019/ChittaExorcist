using System;
using ChittaExorcist.GameCore;
using Cinemachine;
using UnityEngine;

namespace ChittaExorcist.ScreenSettings
{
    public class InitializeScene : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera vCame;
        private void Start()
        {
            // CameraManager.Instance.UpdateCurrentVirtualCamera();
            // CameraManager.Instance.SetVirtualCameraTarget(PlayerManager.Instance.GetPlayerHolderTransform());
            // vCame.Follow = PlayerManager.Instance.GetPlayerHolderTransform();
            // vCame.Priority = 1;
            // CameraManager.Instance.SetCurrentVirtualCameraPriority(0);
        }
    }
}