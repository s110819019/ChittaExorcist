using System;
using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    [Serializable]
    public class PlayerIdleStateData : PlayerStateData
    {
        // [field: SerializeField] public int TestInt { get; private set; }
        [field: SerializeField] public float CameraCheckTime { get; private set; } = 1.0f;
    }
}