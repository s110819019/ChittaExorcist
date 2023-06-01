using System;
using UnityEngine;

namespace ChittaExorcist.Structs
{
    [Serializable]
    public struct KnockbackDetails
    {
        public float KnockbackStrength;
        public Vector2 KnockbackAngle;
        public int KnockbackDirection;
    }
}