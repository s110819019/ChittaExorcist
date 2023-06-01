using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityGravityData : PlayerAbilityComponentData<AbilityGravityPhaseData>
    {
        [field: SerializeField] public float InitialGravityScale { get; private set; }
        
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(AbilityGravity);
        }
    }
}