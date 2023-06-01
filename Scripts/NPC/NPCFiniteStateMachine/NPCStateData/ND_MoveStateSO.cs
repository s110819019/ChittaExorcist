using UnityEngine;

namespace ChittaExorcist.NPCSettings.FSM
{
    [CreateAssetMenu(fileName = "NewNPCMoveStateData", menuName = "Custom Data/NPC/NPC State Data/Move State SO")]
    public class ND_MoveStateSO : NPCStateDataSO
    {
        public float movementSpeed = 3.0f;
    }
}