using UnityEngine;

namespace ChittaExorcist.Common.Variables
{
    [CreateAssetMenu(fileName = "NewFloatVariable", menuName = "Custom Data/Variable/Float SO")]
    public class FloatVariable : ScriptableObject
    {
        [field: SerializeField] public float Value { get; private set; }

        public void SetValue(float newValue) => Value = newValue;

        public void SetValue(FloatVariable newVariable) => Value = newVariable.Value;
        
        public void ApplyChange(float amount) => Value += amount;

        public void ApplyChange(FloatVariable amount) => Value += amount.Value;
    }
}
