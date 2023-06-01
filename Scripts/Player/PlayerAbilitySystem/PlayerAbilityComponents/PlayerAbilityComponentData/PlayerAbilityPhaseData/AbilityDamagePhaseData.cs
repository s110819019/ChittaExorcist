using System;
using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityDamagePhaseData : PlayerAbilityPhaseData
    {
        [field: SerializeField] public float Amount { get; private set; }
        [field: SerializeField] public AudioDataSO AudioData { get; private set; }
    }
}