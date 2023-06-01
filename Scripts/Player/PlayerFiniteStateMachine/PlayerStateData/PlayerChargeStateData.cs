using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public class PlayerChargeStateData : PlayerStateData
    {
        [field: SerializeField] public float MinChargeRequireTime { get; private set; }
        [field: SerializeField] public float PerHealRequireTime { get; private set; }
        [field: SerializeField] public float HealthValueToRecover { get; private set; }
        [field: SerializeField] public float ManaValueToCost { get; private set; }
        
        [field: SerializeField, Header("Stiff Time")] public float RecoverDoneStiffTime { get; private set; }
        [field: SerializeField] public float RecoverOutStiffTime { get; private set; }
    }
}