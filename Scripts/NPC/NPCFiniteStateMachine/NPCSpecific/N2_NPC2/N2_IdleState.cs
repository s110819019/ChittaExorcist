using UnityEngine;

namespace ChittaExorcist.NPCSettings.FSM.NPCSpecific.N2_NPC2
{
    public class N2_IdleState : N_IdleState<NPC2>
    {
        public N2_IdleState(string animationBoolName, NPC2 npc, ND_IdleStateSO stateData) : base(animationBoolName, npc, stateData)
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
            
            // if (DialogueManager.GetInstance().IsPlayingDialogue)
            // {
            //     return;
            // }
            //
            // if (IsIdleTimeOver)
            // {
            //     // Move
            //     StateMachine.ChangeState(NPC.MoveState);
            // }
        }    

        #endregion
    }
}