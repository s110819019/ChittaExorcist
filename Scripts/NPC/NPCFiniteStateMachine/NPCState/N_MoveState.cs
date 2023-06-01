using ChittaExorcist.CharacterCore;
using UnityEngine;

namespace ChittaExorcist.NPCSettings.FSM
{
    public class N_MoveState<T1> : NPCState<T1, ND_MoveStateSO> where T1 : NPC
    {
        public N_MoveState(string animationBoolName, T1 npc, ND_MoveStateSO stateData) : base(animationBoolName, npc, stateData)
        {
        }
        
        #region w/ Core Components
    
        // Movement
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);
        private Movement _movement;
        // CollisionSenses
        protected CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses : Core.GetCoreComponent(out _collisionSenses);
        private CollisionSenses _collisionSenses;    

        #endregion
    
        #region w/ Collision Check

        protected bool IsDetectingWall;
        protected bool IsDetectingLedge;

        #endregion
    
        #region w/ State Workflow

        protected override void DoChecks()
        {
            base.DoChecks();
            
            if (!CollisionSenses)
            {
                Debug.LogWarning("NPC Move State 無法取得 CollisionSenses");
                return;
            }
            
            IsDetectingLedge = CollisionSenses.LedgeVertical;
            IsDetectingWall = CollisionSenses.WallFront;
        }
    
        public override void Enter()
        {
            base.Enter();
            
            if (!Movement)
            {
                Debug.LogWarning("NPC Move State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityX(StateData.movementSpeed * Movement.FacingDirection);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (!Movement)
            {
                Debug.LogWarning("NPC Move State 無法取得 Movement");
                return;
            }
            
            Movement.SetVelocityX(StateData.movementSpeed * Movement.FacingDirection);
        }    

        #endregion
    }
}