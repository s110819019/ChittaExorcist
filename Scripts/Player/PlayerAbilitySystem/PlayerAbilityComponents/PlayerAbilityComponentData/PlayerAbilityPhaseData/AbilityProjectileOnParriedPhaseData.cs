using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityProjectileOnParriedPhaseData : PlayerAbilityPhaseData
    {
        [field: SerializeField] public float TravelTime { get; private set; }
        [field: SerializeField] public AnimationCurve SpeedCurve { get; private set; }
    }
}