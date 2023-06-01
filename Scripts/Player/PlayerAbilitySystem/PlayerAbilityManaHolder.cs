using System.Collections.Generic;
using ChittaExorcist.Common.Variables;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    public class PlayerAbilityManaHolder : MonoBehaviour
    {
        
        #region w/ Mana

        [field: SerializeField, Header("Recover Mana")] public bool IsRecoverMana { get; private set; }
        [field: SerializeField] public List<FloatReference> ManaRecoverList { get; private set; }
        
        [field: SerializeField, Header("Cost Mana")] public bool IsCostMana { get; private set; }
        [field: SerializeField] public List<FloatReference> ManaCostList { get; private set; }

        // public float ManaRequireRecover()
        // {
        //     return ManaRecoverList[CurrentPhaseCounter].Value;
        // }
        //
        // public float ManaRequireCost()
        // {
        //     return ManaCostList[CurrentPhaseCounter].Value;
        // }

        #endregion
    }
}