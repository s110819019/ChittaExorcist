using System;
using UnityEngine;

namespace ChittaExorcist.Common.Variables
{
    [Serializable]
    public class FloatReference
    {
        // Const
        // [field: SerializeField] public bool UseConstant { get; private set; }
        // [field: SerializeField] public float ConstantValue { get; private set; }
        
        [field: SerializeField] public FloatVariable Variable { get; private set; }
    
        // public FloatReference() { }
        // public FloatReference(float value)
        // {
        //     UseConstant = true;
        //     ConstantValue = value;
        // }

        // public float Value => UseConstant ? ConstantValue : Variable.Value;
        public float Value => Variable.Value;

        public static implicit operator float(FloatReference reference) => reference.Value;
    }
}