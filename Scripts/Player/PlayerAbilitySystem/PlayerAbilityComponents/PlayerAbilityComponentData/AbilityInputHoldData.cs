using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityInputHoldData : PlayerAbilityComponentData<AbilityInputHoldPhaseData>
    {
        [field: SerializeField] public TargetHoldInput TargetInput { get; private set; }
        
        public enum TargetHoldInput
        {
            AttackInput,
            BlockInput
        }

        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(AbilityInputHold);
        }
    }
}
