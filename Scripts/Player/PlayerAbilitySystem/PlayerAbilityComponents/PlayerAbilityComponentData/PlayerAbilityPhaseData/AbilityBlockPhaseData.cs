using System;
using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityBlockPhaseData : PlayerAbilityPhaseData
    {
        [field: Header("Particle"), SerializeField] public GameObject BlockParticle { get; private set; }
        [field: SerializeField] public Vector2 BlockParticleOffset { get; private set; }

        [field: Header("Absorption"), SerializeField, Range(0.0f, 1.0f)] public float DamageAbsorption { get; private set; }
        [field: SerializeField, Range(0.0f, 1.0f)] public float KnockbackStrengthAbsorption { get; private set; }
        
        [field: SerializeField] public float BlockWindowStart { get; private set; }
        [field: SerializeField] public float BlockWindowEnd { get; private set; }
        
        [field: Header("Block Direction"), SerializeField] public BlockDirection[] BlockDirections { get; private set; }
        [field: SerializeField] public AudioDataSO AudioData { get; private set; }

    }

    [Serializable]
    public struct BlockDirection
    {
        [field: Range(-180.0f, 180.0f), SerializeField] public float MinAngle { get; private set; }

        [field: Range(-180.0f, 180.0f), SerializeField] public float MaxAngle { get; private set; }
    }
}
