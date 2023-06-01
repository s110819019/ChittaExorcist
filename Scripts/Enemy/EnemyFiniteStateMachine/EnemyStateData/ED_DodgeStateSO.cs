using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    [CreateAssetMenu(fileName = "NewEnemyDodgeStateData", menuName = "Custom Data/Enemy/Enemy State Data/Dodge State SO")]
    public class ED_DodgeStateSO : EnemyStateDataSo
    {
        public float dodgeTime = 0.8f;
        public float dodgeCooldown = 2.0f;
        public AnimationCurve speedCurve;
        public Vector2 dodgeAngle;
    }
}