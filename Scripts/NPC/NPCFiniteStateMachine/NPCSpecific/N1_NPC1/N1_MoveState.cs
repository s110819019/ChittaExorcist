using UnityEngine;

namespace ChittaExorcist.NPCSettings.FSM
{
    public class N1_MoveState : N_MoveState<NPC1>
    {
        public N1_MoveState(string animationBoolName, NPC1 npc, ND_MoveStateSO stateData) : base(animationBoolName, npc, stateData)
        {
        }
        
        public bool IsAtPatrolPoint => NPC.transform.position.x <= NPC.patrolPoint1.position.x ||
                                       NPC.transform.position.x >= NPC.patrolPoint2.position.x;
    
        #region w/ State Workflow

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!IsDetectingLedge || IsDetectingWall)
            {
                // Idle
                NPC.IdleState.SetFlipAfterIdle(true); // 走到邊緣 須返回走
                StateMachine.ChangeState(NPC.IdleState);
            }
            // 左方
            else if (NPC.transform.position.x <= NPC.patrolPoint1.position.x && CollisionSenses.LedgeVerticalCheck.transform.position.x < NPC.patrolPoint1.position.x)
            {
                // Idle

                NPC.IdleState.SetFlipAfterIdle(true); // 走到邊緣 須返回走
                StateMachine.ChangeState(NPC.IdleState);
            }
            // 右方
            else if (NPC.transform.position.x >= NPC.patrolPoint2.position.x && CollisionSenses.LedgeVerticalCheck.transform.position.x > NPC.patrolPoint2.position.x)
            {
                // Idle

                NPC.IdleState.SetFlipAfterIdle(true); // 走到邊緣 須返回走
                StateMachine.ChangeState(NPC.IdleState);
            }
        }    

        #endregion
    }
}