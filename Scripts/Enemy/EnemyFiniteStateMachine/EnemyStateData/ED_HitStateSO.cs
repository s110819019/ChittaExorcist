using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    [CreateAssetMenu(fileName = "NewEnemyHitStateData", menuName = "Custom Data/Enemy/Enemy State Data/Hit State SO")]
    public class ED_HitStateSO : EnemyStateDataSo
    {
        public float getBeatenTime = 0.2f;
    }
}