using UnityEngine;

using ChittaExorcist.Structs;

namespace ChittaExorcist.Common.Interfaces
{
    public interface IProjectile
    {
        public Vector2 StartPosition { get; }
        public Vector2 StartDirection { get; }
        
        public float TravelTime { get; }
        public AnimationCurve SpeedCurve { get; }
        
        public float Damage { get; }
        public float PoiseDamage { get; }
        
        public float KnockbackStrength { get; }
        public Vector2 KnockbackDirection { get; }
        
        public void SetProjectileStartingState(ProjectileDetails details);
    }
}