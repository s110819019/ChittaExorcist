using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
[Serializable]
public class AbilityProjectileOnParriedData : PlayerAbilityComponentData<AbilityProjectileOnParriedPhaseData>
{
    [field: SerializeField] public LayerMask ProjectileDetectableLayers { get; private set; }

    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(AbilityProjectileOnParried);
    }
}
}
