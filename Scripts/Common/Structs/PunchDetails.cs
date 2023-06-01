using System;
using UnityEngine;

namespace ChittaExorcist.Structs
{
    [Serializable]
    public struct PunchDetails
    {
        public Vector3 punchDirection;
        public float punchDuration;
        public int punchVibrato;
        [Range(0.0f, 1.0f)] public float punchElasticity;
    }
}