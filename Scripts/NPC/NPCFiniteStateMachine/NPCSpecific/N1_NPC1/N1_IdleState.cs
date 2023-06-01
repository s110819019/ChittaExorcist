using ChittaExorcist.GameCore.DialogueSettings;
using UnityEngine;

namespace ChittaExorcist.NPCSettings.FSM
{
    public class N1_IdleState : N_IdleState<NPC1>
    {
        public N1_IdleState(string animationBoolName, NPC1 npc, ND_IdleStateSO stateData) : base(animationBoolName, npc, stateData)
        {
        }
        
        #region w/ State Workflow

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // TODO: 不太好
            if (DialogueManager.Instance.IsPlayingDialogue)
            {
                return;
            }
        
            if (IsIdleTimeOver)
            {
                // Move
                StateMachine.ChangeState(NPC.MoveState);
            }
        }    

        #endregion
    }
}