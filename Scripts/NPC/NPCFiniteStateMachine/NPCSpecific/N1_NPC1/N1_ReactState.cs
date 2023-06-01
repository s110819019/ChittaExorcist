using UnityEngine;

namespace ChittaExorcist.NPCSettings.FSM
{
    public class N1_ReactState : N_ReactState<NPC1>
    {
        public N1_ReactState(string animationBoolName, NPC1 npc, ND_ReactStateSO stateData) : base(animationBoolName, npc, stateData)
        {
        }
        
        #region w/ State Workflow
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!IsAnimationFinished) return;
        
            StateMachine.ChangeState(NPC.IdleState);
        }    

        #endregion
    }
}