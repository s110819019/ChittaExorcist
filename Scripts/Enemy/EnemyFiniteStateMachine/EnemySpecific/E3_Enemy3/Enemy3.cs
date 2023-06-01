using ChittaExorcist.CharacterCore;
using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class Enemy3 : Enemy
    {
        #region w/ Components

        [SerializeField] private Transform meleeAttackPosition;

        #endregion
        
        #region w/ Core Components
        
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
            Stats.OnHealthZero += ChangeToDeadState;
            DamageReceiver.OnHit += CheckGetDamaged;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            Stats.OnHealthZero -= ChangeToDeadState;
            DamageReceiver.OnHit -= CheckGetDamaged;
        }

        #endregion

        #region w/ Event React
        
        private void CheckGetDamaged()
        {
            if (!IsPlayerFront)
            {
                // Not => Hit, Attack, Dead
                if (StateMachine.CurrentState != MeleeAttackState && StateMachine.CurrentState != DeadState)
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

        #endregion
        
        #region w/ State Variables

        public E3_IdleState IdleState { get; private set; }
        public E3_MoveState MoveState { get; private set; }
        public E3_ChargeState ChargeState { get; private set; }
        public E3_PlayerDetectedState PlayerDetectedState { get; private set; }
        public E3_LookForPlayerState LookForPlayerState { get; private set; }
        public E3_MeleeAttackState MeleeAttackState { get; private set; }
        public E3_DeadState DeadState { get; private set; }

        #endregion
        
        #region w/ State Datas

        [Header("State Data")] [Header("Idle")] [SerializeField] private ED_IdleStateSO idleStateData;

        [Header("Move")] [SerializeField] private ED_MoveStateSO moveStateData;
        
        [Header("Charge")] [SerializeField] private ED_ChargeStateSO chargeStateData;

        [Header("Player Detected")] [SerializeField] private ED_PlayerDetectedStateSO playerDetectedStateData;
        
        [Header("Look for Player")] [SerializeField] private ED_LookForPlayerStateSO lookForPlayerStateData;

        [Header("Melee Attack")] [SerializeField] private ED_MeleeAttackStateSO meleeAttackStateData;

        [Header("Dead")] [SerializeField] private ED_DeadStateSO deadStateData;
        
        #endregion
        
        #region w/ Init States

        protected override void InitializeStates()
        {
            base.InitializeStates();
            IdleState = new E3_IdleState("idle", this, idleStateData);
            MoveState = new E3_MoveState("move", this, moveStateData);
            ChargeState = new E3_ChargeState("charge", this, chargeStateData);
            PlayerDetectedState = new E3_PlayerDetectedState("playerDetected", this, playerDetectedStateData);
            LookForPlayerState = new E3_LookForPlayerState("lookForPlayer", this, lookForPlayerStateData);
            MeleeAttackState = new E3_MeleeAttackState("meleeAttack", this, meleeAttackStateData, meleeAttackPosition);
            DeadState = new E3_DeadState("dead", this, deadStateData);
        }

        #endregion
        
        #region w/ Unity Callback Functions

        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(IdleState);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StateMachine.ChangeState(IdleState);
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