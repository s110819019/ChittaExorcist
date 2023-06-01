using System;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityDamageData : PlayerAbilityComponentData<AbilityDamagePhaseData>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(AbilityDamage);
        }
    }
}