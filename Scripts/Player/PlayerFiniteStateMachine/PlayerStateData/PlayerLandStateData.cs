using System;
using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    [Serializable]
    public class PlayerLandStateData : PlayerStateData
    {
        [field: SerializeField] public float MaxLandingTime { get; private set; }
        [field: SerializeField] public AudioDataSO AudioData { get; private set; }
    }
}