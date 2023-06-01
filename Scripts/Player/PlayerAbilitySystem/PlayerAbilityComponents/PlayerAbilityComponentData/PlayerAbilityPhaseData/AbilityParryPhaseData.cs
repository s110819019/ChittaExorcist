using System;
using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityParryPhaseData : PlayerAbilityPhaseData
    {
        [field: Header("Particle"), SerializeField] public GameObject ParryParticle { get; private set; }
        [field: SerializeField] public Vector2 ParryParticleOffset { get; private set; }
        
        [field: Header("Absorption"), SerializeField, Range(0.0f, 1.0f)] public float DamageAbsorption { get; private set; }
        [field: SerializeField, Range(0.0f, 1.0f)] public float KnockbackStrengthAbsorption { get; private set; }

        // [field: SerializeField] public float ParryWindowStart { get; private set; }
        // [field: SerializeField] public float ParryWindowStop { get; private set; }
        
        [Header("Parry Box")]
        public bool debug;
        [field: SerializeField] public Rect ParryHitBox { get; private set; }
        [field: SerializeField] public float ParryMaxDelayTime { get; private set; } = 1.0f;

        [field: Header("Camera Shake Arguments"), SerializeField] public float CameraShakeStrength { get; private set; }
        [field: SerializeField] public float CameraShakeFrequency { get; private set; }
        [field: SerializeField] public float CameraShakeDuration { get; private set; } = 0.2f;
        [field: SerializeField] public AudioDataSO AudioData { get; private set; }
    }
}