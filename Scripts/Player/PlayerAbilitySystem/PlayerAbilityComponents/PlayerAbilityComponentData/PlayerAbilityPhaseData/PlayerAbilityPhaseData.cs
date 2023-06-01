using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class PlayerAbilityPhaseData
    {
        [SerializeField, HideInInspector] private string elementName;

        // e.g. Phase 1
        public void SetPhaseName(int i) => elementName = $"Phase {i}";
    }
}