using System;
using ChittaExorcist.CharacterCore;
using ChittaExorcist.Common;
using ChittaExorcist.Structs;
using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public abstract class Enemy : MonoBehaviour
    {
        // public bool ShouldGetKnockbackAfterGetParry { get; private set; }
        //
        // public void SetShouldGetKnockbackAfterGetParry(bool value)
        // {
        //     ShouldGetKnockbackAfterGetParry = value;
        // }

        // public bool GetParryShouldKnockback { get; private set; }
        // public KnockbackDetails ParryKnockbackDetails { get; private set; }

        public ParriedDetails ParriedDetails;

        public void SetParriedDetails(ParriedDetails parriedDetails)
        {
            if (parriedDetails.IsSetParriedKnockback)
            {
                ParriedDetails.ParriedKnockbackDetails = parriedDetails.ParriedKnockbackDetails;
            }
        }


        // public virtual void SetGetParryShouldKnockback(bool value)
        // {
        //     GetParryShouldKnockback = value;
        // }
        
        // public void SetParryKnockbakeInfo(KnockbackDetails details)
        // {
        //     ParryKnockbackDetails = details;
        // }
        
        #region w/ Enemy Data

        [Header("Enemy Data")]
        [SerializeField] private EnemyDataSO data;

        public EnemyDataSO Data
        {
            get => data;
            private set => data = value;
        }

        [Header("Other Data")]
        [SerializeField] private Transform playerCheck;

        #endregion

        #region w/ Event Subscribe

        protected virtual void SetSubscribeEvents() { }
        protected virtual void SetUnsubscribeEvents() { }

        #endregion

        #region w/ Components

        public Animator Animator { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        public Core Core { get; private set; }

        public EnemyAnimationEventHandler EventHandler { get; private set; }

        #endregion

        #region w/ State

        public EnemyStateMachine StateMachine { get; private set; }
        protected virtual void InitializeStates() { }

        #endregion

        #region w/ Variables

        protected bool IsPlayerFront;
        public int LastDamageDirection { get; private set; }

        #endregion

        #region w/ Core Components

        // Movement
        protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);
        private Movement _movement;

        public KnockbackReceiver KnockbackReceiver =>
            _knockbackReceiver ? _knockbackReceiver : Core.GetCoreComponent(out _knockbackReceiver);
        private KnockbackReceiver _knockbackReceiver;
        
        protected MaterialManager MaterialManager => _materialManager ? _materialManager : Core.GetCoreComponent(out _materialManager);
        private MaterialManager _materialManager;

        #endregion
        
        #region w/ Unity Callback Functions

        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            SpriteRenderer = GetComponent<SpriteRenderer>();

            Core = GetComponentInChildren<Core>();

            StateMachine = new EnemyStateMachine();
            
            EventHandler = GetComponent<EnemyAnimationEventHandler>();
            
            InitializeStates();
        }

        protected virtual void Start()
        {
            MaterialManager.InitializeSpriteRenderer(SpriteRenderer);
        }

        protected virtual void Update()
        {
            Core.LogicUpdate();
            StateMachine.CurrentState.LogicUpdate();
        }

        protected virtual void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
            
            // TODO: 是否要修改
            // 確認玩家是否在視野前方
            IsPlayerFront = CheckPlayerInCloseRangeAction() || CheckPlayerInMinAggroRange() || CheckPlayerInMaxAggroRange();
        }

        protected virtual void OnEnable()
        {
            SetSubscribeEvents();
        }

        protected virtual void OnDisable()
        {
            SetUnsubscribeEvents();
        }
        
        #endregion

    #region w/ Player Check
    
    // 最近仇恨範圍 須判斷有無障礙物
    public virtual bool CheckPlayerInMinAggroRange()
    {
        // 擊中玩家
        var playerHit = Physics2D.Raycast(playerCheck.position, transform.right, Data.minAggroDistance,
            Data.whatIsPlayer);
        // 擊中地形
        var groundHit = Physics2D.Raycast(playerCheck.position, transform.right, Data.minAggroDistance,
            Data.whatIsGround);
        if (!playerHit.collider || !groundHit.collider) return playerHit;
        // 同時擊中玩家與地形
        var playerHitDistance = playerHit.distance;
        var groundHitDistance = groundHit.distance;
        // 比較距離 確認沒先擊中地形
        return playerHitDistance < groundHitDistance;
        
        // return Physics2D.Raycast(playerCheck.position, transform.right, EntityData.minAggroDistance,
        //     EntityData.whatIsPlayer);
    }
    // 最遠仇恨範圍 須判斷有無障礙物
    public virtual bool CheckPlayerInMaxAggroRange()
    {
        // 擊中玩家
        var playerHit = Physics2D.Raycast(playerCheck.position, transform.right, Data.maxAggroDistance,
            Data.whatIsPlayer);
        // 擊中地形
        var groundHit = Physics2D.Raycast(playerCheck.position, transform.right, Data.maxAggroDistance,
            Data.whatIsGround);
        if (!playerHit.collider || !groundHit.collider) return playerHit;
        // 同時擊中玩家與地形
        var playerHitDistance = playerHit.distance;
        var groundHitDistance = groundHit.distance;
        // 比較距離 確認沒先擊中地形
        return playerHitDistance < groundHitDistance;
        
        // return Physics2D.Raycast(playerCheck.position, transform.right, EntityData.maxAggroDistance,
        //     EntityData.whatIsPlayer);
    }
    // 近距離動作範圍
    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, Data.closeRangeActionDistance,
            Data.whatIsPlayer);
    }    

    #endregion

    #region w/ Gizmos

    public virtual void OnDrawGizmos()
    {
        if (Core != null)
        {
            // Gizmos.DrawLine(CollisionSenses.WallCheck.position, CollisionSenses.WallCheck.position + (Vector3)(Vector2.right * Movement.FacingDirection * CollisionSenses.WallCheckDistance));
            // Gizmos.DrawLine(CollisionSenses.LedgeVerticalCheck.position, CollisionSenses.LedgeVerticalCheck.position + (Vector3)(Vector2.down * CollisionSenses.WallCheckDistance));
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere((playerCheck.position + (Vector3)(Vector2.right * Data.closeRangeActionDistance * Movement.FacingDirection)), 0.2f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((playerCheck.position + (Vector3)(Vector2.right * Data.maxAggroDistance * Movement.FacingDirection)), 0.2f);
            Gizmos.DrawWireSphere((playerCheck.position + (Vector3)(Vector2.right * Data.minAggroDistance * Movement.FacingDirection)), 0.2f);
        }
    }    

    #endregion
    }
}
