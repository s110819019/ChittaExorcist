using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityDamageEffectPhaseData : PlayerAbilityPhaseData
    {
        [field: SerializeField] public GameObject Particle { get; private set; }
        [field: SerializeField] public Vector2 Offset { get; private set; }
        
        [field: SerializeField] public bool UseRandomRotate { get; private set; }
        [field: SerializeField, Range(-360.0f, 360.0f)] public float MinAngle { get; private set; }
        [field: SerializeField, Range(-360.0f, 360.0f)] public float MaxAngle { get; private set; }
    }
}