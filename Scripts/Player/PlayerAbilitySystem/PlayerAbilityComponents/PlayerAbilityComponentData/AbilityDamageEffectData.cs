using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityDamageEffectData : PlayerAbilityComponentData<AbilityDamageEffectPhaseData>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(AbilityDamageEffect);
        }
    }
}