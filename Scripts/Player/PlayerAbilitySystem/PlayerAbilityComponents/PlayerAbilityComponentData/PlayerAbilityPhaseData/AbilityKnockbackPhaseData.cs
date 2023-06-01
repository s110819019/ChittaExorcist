using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityKnockbackPhaseData : PlayerAbilityPhaseData
    {
        [field: SerializeField] public Vector2 KnockbackAngle { get; private set; }
        [field: SerializeField] public float KnockbackStrength { get; private set; }
    }
}