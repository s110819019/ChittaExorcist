using System;
using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    [Serializable]
    public class PlayerJumpStateData : PlayerStateData
    {
        [field: SerializeField] public int AmountOfJumps { get; private set; }
        [field: SerializeField] public float JumpVelocity { get; private set; }
        [field: SerializeField] public AudioDataSO AudioData { get; private set; }

    }
}