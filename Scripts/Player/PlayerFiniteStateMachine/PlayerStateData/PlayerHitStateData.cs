using System;
using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    [Serializable]
    public class PlayerHitStateData : PlayerStateData
    {
        [field: SerializeField] public float MaxHitTime { get; private set; }
        [field: SerializeField] public AudioDataSO AudioData { get; private set; }
    }
}