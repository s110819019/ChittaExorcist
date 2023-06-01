using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityKnockbackData : PlayerAbilityComponentData<AbilityKnockbackPhaseData>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(AbilityKnockback);
        }
    }
}