using ChittaExorcist.CharacterCore;
using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class Enemy2 : Enemy
    {
        #region w/ Components

        [SerializeField] private Transform meleeAttackPosition;
        [SerializeField] private Transform rangedAttackPosition;
        [SerializeField] private Transform obstacleBlockPosition;

        #endregion

        #region w/ Core Components

        // Poise
        protected PoiseReceiver PoiseReceiver => _poiseReceiver ? _poiseReceiver : Core.GetCoreComponent(out _poiseReceiver);
        private PoiseReceiver _poiseReceiver;
        // Damage
        protected DamageReceiver DamageReceiver => _damageReceiver ? _damageReceiver : Core.GetCoreComponent(out _damageReceiver);
        private DamageReceiver _damageReceiver;
        // Stats
        protected Stats Stats => _stats ? _stats : Core.GetCoreComponent(out _stats);
        private Stats _stats;
        
        #endregion

        #region w/ Event Subscribe

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            // Combat.OnGetDamaged += CheckGetDamaged;
            // Stats.OnHealthZero += ChangeToDeadState;
            // PoiseReceiver.OnStun += ChangeToStunState;
            DamageReceiver.OnHit += CheckGetDamaged;
            Stats.OnHealthZero += ChangeToDeadState;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            // Combat.OnGetDamaged -= CheckGetDamaged;
            // Stats.OnHealthZero -= ChangeToDeadState;
            // PoiseReceiver.OnStun -= ChangeToStunState;
            DamageReceiver.OnHit -= CheckGetDamaged;
            Stats.OnHealthZero -= ChangeToDeadState;
        }

        #endregion

        private void CheckGetDamaged()
        {
            if (!IsPlayerFront)
            {
                // Not => Attack, Dead
                if (StateMachine.CurrentState != MeleeAttackState && StateMachine.CurrentState != DeadState &&
                    StateMachine.CurrentState != RangedAttackState && StateMachine.CurrentState != DodgeState)
                {
                    LookForPlayerState.SetTurnImmediately(true);
                    StateMachine.ChangeState(LookForPlayerState);
                }
            }
        }
        
        private void ChangeToDeadState()
        {
            // Debug.Log("Enemy1 Dead");
            StateMachine.ChangeState(DeadState);
        }
        
        // public bool ShouldChangeToStun { get; private set; }
        // private void ChangeToStunState()
        // {
        //     // if (StateMachine.CurrentState != StunState)
        //     // {
        //     //     StateMachine.ChangeState(StunState);
        //     // }
        //     ShouldChangeToStun = true;
        // }

        // public void ResetShouldChangeToStun() => ShouldChangeToStun = false;

        #region w/ State Variables

        public E2_IdleState IdleState { get; private set; }
        public E2_MoveState MoveState { get; private set; }
        public E2_PlayerDetectedState PlayerDetectedState { get; private set; }
        public E2_LookForPlayerState LookForPlayerState { get; private set; }
        public E2_MeleeAttackState MeleeAttackState { get; private set; }
        // public E2_HitState HitState { get; private set; }
        public E2_RangedAttackState RangedAttackState { get; private set; }
        public E2_StunState StunState { get; private set; }
        public E2_DodgeState DodgeState { get; private set; }
        // public E2_ObstacleBlockState ObstacleBlockState { get; private set; }
        //
        public E2_DeadState DeadState { get; private set; }

        #endregion

        #region w/ State Datas

        [Header("State Data")] [Header("Idle")] [SerializeField] private ED_IdleStateSO idleStateData;

        [Header("Move")] [SerializeField] private ED_MoveStateSO moveStateData;

        [Header("Player Detected")] [SerializeField] private ED_PlayerDetectedStateSO playerDetectedStateData;
        
        [Header("Look for Player")] [SerializeField] private ED_LookForPlayerStateSO lookForPlayerStateData;

        [Header("Melee Attack")] [SerializeField] private ED_MeleeAttackStateSO meleeAttackStateData;

        // [Header("Hit")] [SerializeField] private ED_HitState hitStateData;
        //
        [Header("Ranged Attack")] [SerializeField] private ED_RangedAttackStateSO rangedAttackStateData;
        //
        [Header("Stun")] [SerializeField] private ED_StunStateSO stunStateData;
        //
        [Header("Dodge")] [SerializeField] public ED_DodgeStateSO dodgeStateData;
        //
        // [Header("Block")] [SerializeField] public ED_ObstacleBlockState obstacleBlockStateData;
        //
        [Header("Dead")] [SerializeField] private ED_DeadStateSO deadStateData;

        #endregion

        #region w/ Init States

        protected override void InitializeStates()
        {
            base.InitializeStates();
            IdleState = new E2_IdleState("idle", this, idleStateData);
            MoveState = new E2_MoveState("move", this, moveStateData);
            PlayerDetectedState = new E2_PlayerDetectedState("playerDetected", this, playerDetectedStateData);
            LookForPlayerState = new E2_LookForPlayerState("lookForPlayer", this, lookForPlayerStateData);
            MeleeAttackState = new E2_MeleeAttackState("meleeAttack", this, meleeAttackStateData, meleeAttackPosition);
            StunState = new E2_StunState("stun", this, stunStateData);






            // HitState = new E2_HitState(this, "hit", hitStateData, this);

            RangedAttackState = new E2_RangedAttackState("rangedAttack", this, rangedAttackStateData, rangedAttackPosition);

            DodgeState = new E2_DodgeState("dodge", this, dodgeStateData);

            // ObstacleBlockState = new E2_ObstacleBlockState(this, "block", obstacleBlockPosition, obstacleBlockStateData, this);

            DeadState = new E2_DeadState("dead", this, deadStateData);
        }

        #endregion

        // #region w/ Get Damage
        //
        // // 玩家不在視線前方 被攻擊時應該要立刻轉身尋找玩家
        // private void CheckGetDamaged()
        // {
        //     if (!IsPlayerFront)
        //         if (StateMachine.CurrentState != HitState && StateMachine.CurrentState != MeleeAttackState &&
        //             StateMachine.CurrentState != RangedAttackState && StateMachine.CurrentState != DeadState)
        //         {
        //             LookForPlayerState.SetTurnImmediately(true);
        //             StateMachine.ChangeState(LookForPlayerState);
        //         }
        // }
        //
        // #endregion

        // #region w/ Death
        //
        // private void ChangeToDeadState()
        // {
        //     // Debug.Log("Dead");
        //     StateMachine.ChangeState(DeadState);
        // }
        //
        // #endregion

        #region w/ Unity Callback Functions

        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(IdleState);
        }

        protected override void Update()
        {
            base.Update();
        }

        #endregion

        #region w/ Gizmos
        
        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
        
            if (meleeAttackPosition != null)
                Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
        }
        
        #endregion
    }
}