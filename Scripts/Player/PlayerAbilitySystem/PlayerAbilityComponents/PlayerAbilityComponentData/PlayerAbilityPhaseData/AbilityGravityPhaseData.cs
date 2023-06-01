using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityGravityPhaseData : PlayerAbilityPhaseData
    {
        [field: SerializeField] public float FloatingGravityScale { get; private set; }
    }
}