using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
[Serializable]
public class AbilityRangedData : PlayerAbilityComponentData<AbilityRangedPhaseData>
{
    [field: SerializeField] public LayerMask DetectableLayers { get; private set; }

    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(AbilityRanged);
    }
}
}
