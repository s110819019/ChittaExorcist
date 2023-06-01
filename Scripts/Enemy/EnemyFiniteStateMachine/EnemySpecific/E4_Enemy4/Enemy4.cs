using ChittaExorcist.CharacterCore;
using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class Enemy4 : Enemy
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
            // if (!IsPlayerFront)
            // {
            //     // Not => Hit, Attack, Dead
            //     if (StateMachine.CurrentState != MeleeAttackState && StateMachine.CurrentState != DeadState)
            //     {
            //         LookForPlayerState.SetTurnImmediately(true);
            //         StateMachine.ChangeState(LookForPlayerState);
            //     }
            // }
        }
        
        private void ChangeToDeadState()
        {
            // Debug.Log("Enemy1 Dead");
            StateMachine.ChangeState(DeadState);
        }
        
        #endregion
        
        #region w/ State Variables

        public E4_IdleState IdleState { get; private set; }
        public E4_PlayerDetectedState PlayerDetectedState { get; private set; }
        public E4_LookForPlayerState LookForPlayerState { get; private set; }
        public E4_MeleeAttackState MeleeAttackState { get; private set; }
        public E4_DeadState DeadState { get; private set; }

        #endregion
        
        #region w/ State Datas

        [Header("State Data")] [Header("Idle")] [SerializeField] private ED_IdleStateSO idleStateData;

        [Header("Player Detected")] [SerializeField] private ED_PlayerDetectedStateSO playerDetectedStateData;
        
        [Header("Look for Player")] [SerializeField] private ED_LookForPlayerStateSO lookForPlayerStateData;

        [Header("Melee Attack")] [SerializeField] private ED_MeleeAttackStateSO meleeAttackStateData;

        [Header("Dead")] [SerializeField] private ED_DeadStateSO deadStateData;
        
        #endregion
        
        #region w/ Init States

        protected override void InitializeStates()
        {
            base.InitializeStates();
            IdleState = new E4_IdleState("idle", this, idleStateData);
            PlayerDetectedState = new E4_PlayerDetectedState("playerDetected", this, playerDetectedStateData);
            LookForPlayerState = new E4_LookForPlayerState("lookForPlayer", this, lookForPlayerStateData);
            MeleeAttackState = new E4_MeleeAttackState("meleeAttack", this, meleeAttackStateData, meleeAttackPosition);
            DeadState = new E4_DeadState("dead", this, deadStateData);
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