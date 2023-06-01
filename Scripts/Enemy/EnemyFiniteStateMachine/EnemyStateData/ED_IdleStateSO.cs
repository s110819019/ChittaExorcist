using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    [CreateAssetMenu(fileName = "NewEnemyIdleStateData", menuName = "Custom Data/Enemy/Enemy State Data/Idle State SO")]
    public class ED_IdleStateSO : EnemyStateDataSo
    {
        public float minIdleTime = 1.0f;
        public float maxIdleTime = 2.0f;
    }
}
