using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    [CreateAssetMenu(fileName = "NewEnemyLookForPlayerStateData", menuName = "Custom Data/Enemy/Enemy State Data/LookForPlayer State SO")]
    public class ED_LookForPlayerStateSO : EnemyStateDataSo
    {
        public int amountOfTurns = 2;
        
        public float timeBetweenTurns = 0.75f;
    }
}