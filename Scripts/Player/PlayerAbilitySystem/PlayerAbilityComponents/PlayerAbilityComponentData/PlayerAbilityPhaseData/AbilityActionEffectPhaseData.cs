using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityActionEffectPhaseData : PlayerAbilityPhaseData
    {
        [field: SerializeField] public GameObject Particle { get; private set; }
        [field: SerializeField] public Vector2 Offset { get; private set; }
    }
}