using System;
using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityActionHitBoxPhaseData : PlayerAbilityPhaseData
    {
        public bool debug;
        [field: SerializeField] public Rect HitBox { get; private set; }
        [field: SerializeField] public AudioDataSO AudioData { get; private set; }
    }
}