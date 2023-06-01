using System;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityBlockData : PlayerAbilityComponentData<AbilityBlockPhaseData>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(AbilityBlock);
        }
    }
}
