using UnityEngine;

namespace ChittaExorcist.NPCSettings.FSM
{
    [CreateAssetMenu(fileName = "NewNPCIdleStateData", menuName = "Custom Data/NPC/NPC State Data/Idle State SO")]
    public class ND_IdleStateSO : NPCStateDataSO
    {
        public float minIdleTime = 1f;
        public float maxIdleTime = 2f;
    }
}