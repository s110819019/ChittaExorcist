using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    [CreateAssetMenu(fileName = "NewEnemyChargeStateData", menuName = "Custom Data/Enemy/Enemy State Data/Charge State SO")]
    public class ED_ChargeStateSO : EnemyStateDataSo
    {
        public float chargeSpeed = 6.0f;
        public float chargeTime = 2.0f;
        
        [Header("For Melee Attack After Charge")]
        public float chargeAttackCooldown = 1.0f;
        public float chargeTimeAfterMeleeAttack = 1.0f;
    }
}