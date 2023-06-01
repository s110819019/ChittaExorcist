using System;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityActionEffectData : PlayerAbilityComponentData<AbilityActionEffectPhaseData>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(AbilityActionEffect);
        }
    }
}