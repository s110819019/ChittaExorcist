using System;
using System.Collections;
using System.Collections.Generic;
using ChittaExorcist.Common.Interfaces;
using UnityEngine;

namespace ChittaExorcist.Structs
{
    [Serializable]
    public struct ProjectileDetails
    {
        // IProjectile
        public Vector2 StartPosition;
        public Vector2 StartDirection;
        public float TravelTime;
        public AnimationCurve SpeedCurve;
        public float Damage;
        public float PoiseDamage;
        public float KnockbackStrength;
        public Vector2 KnockbackDirection;

        public bool UseFallGravity;
        public bool ShouldFlip;
        public Transform ProjectileUser;
        public LayerMask InteractableLayers;
    }
}
