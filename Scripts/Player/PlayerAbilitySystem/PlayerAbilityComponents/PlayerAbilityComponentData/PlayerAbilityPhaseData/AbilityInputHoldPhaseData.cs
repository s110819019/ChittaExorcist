using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityInputHoldPhaseData : PlayerAbilityPhaseData
    {
        // [field: SerializeField] public int NewValue { get; private set; }
        // [field: SerializeField] public TargetHoldInput TargetInput { get; private set; }
        [field: SerializeField] public float MaxHoldTime { get; private set; }
    }

    // public enum TargetHoldInput
    // {
    //     AttackInput,
    //     BlockInput
    // }
}
