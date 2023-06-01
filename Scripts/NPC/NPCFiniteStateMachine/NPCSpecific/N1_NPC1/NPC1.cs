using UnityEngine;

namespace ChittaExorcist.NPCSettings.FSM
{
    public class NPC1 : NPC
    {
        #region w/ Event Subscribe

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
        }

        #endregion

        #region w/ Dialogue Trigger

        protected override void OnDialogueEnter()
        {
            base.OnDialogueEnter();
            // Debug.Log("React");
            if (MoveState.IsAtPatrolPoint)
            {
                IdleState.SetFlipAfterIdle(false);
            }

            StateMachine.ChangeState(ReactState);
        }

        protected override void OnDialogueEnterCheckPlayerTransform(Transform targetTransform)
        {
            base.OnDialogueEnterCheckPlayerTransform(targetTransform);
            // 朝向右方 而 玩家在左方
            if (Movement.FacingDirection == 1 && targetTransform.position.x < transform.position.x)
            {
                Movement.Flip();
                IdleState.SetFlipAfterIdle(true);
            }
            // 朝向左方 而 玩家在右方
            else if (Movement.FacingDirection == -1 && targetTransform.position.x > transform.position.x)
            {
                Movement.Flip();
                IdleState.SetFlipAfterIdle(true);
            }
            // else
            // {
            //     IdleState.SetFlipAfterIdle(false);
            // }
        }

        #endregion

        #region w/ State Variables

        public N1_IdleState IdleState { get; private set; }
        public N1_MoveState MoveState { get; private set; }
        public N1_ReactState ReactState { get; private set; }

        #endregion

        #region w/ State Data

        [Header("State Data")] [Header("Idle")] [SerializeField]
        private ND_IdleStateSO idleStateData;

        [Header("Move")] [SerializeField] private ND_MoveStateSO moveStateData;
        [Header("React")] [SerializeField] private ND_ReactStateSO reactStateData;

        #endregion

        #region w/ Init States

        protected override void InitializeStates()
        {
            base.InitializeStates();
            IdleState = new N1_IdleState("idle", this, idleStateData);
            MoveState = new N1_MoveState("move", this, moveStateData);
            ReactState = new N1_ReactState("react", this, reactStateData);
        }

        #endregion

        #region w/ Unity Callback Functions

        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(IdleState);
        }

        #endregion
    }
}