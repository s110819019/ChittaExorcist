using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
[Serializable]
public class AbilityKnockbackOnParriedData : PlayerAbilityComponentData<AbilityKnockbackOnParriedPhaseData>
{
    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(AbilityKnockbackOnParried);
    }
}
}
