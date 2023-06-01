using System.Collections;
using System.Collections.Generic;
using ChittaExorcist.CharacterCore;
using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class Enemy1 : Enemy
    {
        #region w/ Components

        [SerializeField] private Transform meleeAttackPosition;

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
            Stats.OnHealthZero += ChangeToDeadState;
            
            
            DamageReceiver.OnHit += ChangeToHitState;
            DamageReceiver.OnHit += CheckGetDamaged;
            
            PoiseReceiver.OnStun += ChangeToStunState;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            // Combat.OnGetDamaged -= CheckGetDamaged;
            Stats.OnHealthZero -= ChangeToDeadState;
            
            DamageReceiver.OnHit -= ChangeToHitState;
            DamageReceiver.OnHit -= CheckGetDamaged;

            PoiseReceiver.OnStun -= ChangeToStunState;
        }

        #endregion

        public bool ShouldChangeToStun { get; private set; }
        private void ChangeToStunState()
        {
            // if (StateMachine.CurrentState != StunState)
            // {
            //     StateMachine.ChangeState(StunState);
            // }
            ShouldChangeToStun = true;
        }

        public void ResetShouldChangeToStun() => ShouldChangeToStun = false;

        #region w/ State Variables

        public E1_IdleState IdleState { get; private set; }
        public E1_MoveState MoveState { get; private set; }
        public E1_ChargeState ChargeState { get; private set; }
        public E1_PlayerDetectedState PlayerDetectedState { get; private set; }
        public E1_LookForPlayerState LookForPlayerState { get; private set; }
        public E1_MeleeAttackState MeleeAttackState { get; private set; }
        public E1_HitState HitState { get; private set; }
        public E1_StunState StunState { get; private set; }
        
        public E1_DeadState DeadState { get; private set; }

        #endregion

        #region w/ State Datas

        [Header("State Data")] [Header("Idle")] [SerializeField] private ED_IdleStateSO idleStateData;

        [Header("Move")] [SerializeField] private ED_MoveStateSO moveStateData;
        
        [Header("Charge")] [SerializeField] private ED_ChargeStateSO chargeStateData;

        [Header("Player Detected")] [SerializeField] private ED_PlayerDetectedStateSO playerDetectedStateData;
        
        [Header("Look for Player")] [SerializeField] private ED_LookForPlayerStateSO lookForPlayerStateData;

        [Header("Melee Attack")] [SerializeField] private ED_MeleeAttackStateSO meleeAttackStateData;

        [Header("Hit")] [SerializeField] private ED_HitStateSO hitStateData;

        [Header("Stun")] [SerializeField] private ED_StunStateSO stunStateData;
        
        [Header("Dead")] [SerializeField] private ED_DeadStateSO deadStateData;


        #endregion

        #region w/ Init States

        protected override void InitializeStates()
        {
            base.InitializeStates();
            IdleState = new E1_IdleState("idle", this, idleStateData);
            MoveState = new E1_MoveState("move", this, moveStateData);
            ChargeState = new E1_ChargeState("charge", this, chargeStateData);
            PlayerDetectedState = new E1_PlayerDetectedState("playerDetected", this, playerDetectedStateData);
            LookForPlayerState = new E1_LookForPlayerState("lookForPlayer", this, lookForPlayerStateData);
            MeleeAttackState = new E1_MeleeAttackState("meleeAttack", this, meleeAttackStateData, meleeAttackPosition);
            StunState = new E1_StunState("stun", this, stunStateData);

            HitState = new E1_HitState("hit", this, hitStateData);


            DeadState = new E1_DeadState("dead", this, deadStateData);
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

        private void ChangeToHitState()
        {
            // if (StateMachine.CurrentState != HitState)
            // {
            //     StateMachine.ChangeState(HitState);
            // }
        }

        // public override void SetGetParryShouldKnockback(bool value)
        // {
        //     base.SetGetParryShouldKnockback(value);
        //     StateMachine.ChangeState(IdleState);
        // }

        private void CheckGetDamaged()
        {
            if (!IsPlayerFront)
            {
                // Not => Hit, Attack, Dead
                if (StateMachine.CurrentState != HitState && StateMachine.CurrentState != MeleeAttackState && StateMachine.CurrentState != DeadState)
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
