using System;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityMovementData : PlayerAbilityComponentData<AbilityMovementPhaseData>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(AbilityMovement);
        }
    }
}