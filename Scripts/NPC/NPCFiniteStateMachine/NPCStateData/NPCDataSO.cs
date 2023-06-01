using UnityEngine;

namespace ChittaExorcist.NPCSettings.FSM
{
    [CreateAssetMenu(fileName = "NewNPCData", menuName = "Custom Data/NPC/NPC Data SO")]
    public class NPCDataSO : ScriptableObject
    {
        public LayerMask whatIsGround;
        public LayerMask whatIsPlayer;
    }
}