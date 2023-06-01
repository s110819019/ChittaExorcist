using System;
using UnityEngine;

using ChittaExorcist.CharacterCore;
using ChittaExorcist.EventChannel;
using ChittaExorcist.PlayerEffectSettings;
using ChittaExorcist.PlayerSettings.InputHandler;

namespace ChittaExorcist.PlayerSettings.FSM
{
    /// <summary>
    /// FSM 的 Player
    /// </summary>
    public abstract class Player : MonoBehaviour
    {
        #region w/ Player Data
        
        [Header("Player Data")]
        [SerializeField] private PlayerDataSO data;
        public PlayerDataSO Data { get => data; private set => data = value; }
        
        public enum PlayerCharacter
        {
            ChuXiao,
            ShaoYue
        }
        public PlayerCharacter Character { get; protected set; }

        #endregion

        #region w/ Event Subscribe

        protected virtual void SetSubscribeEvents()
        {
            // 訂閱事件
        }

        protected virtual void SetUnsubscribeEvents()
        {
            // 取消訂閱事件
        }
        
        #endregion

        #region w/ Components
        
        public Animator Animator { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        public Core Core { get; private set; }
        
        public PlayerHolder PlayerHolder { get; private set; }

        #endregion

        #region w/ State

        public PlayerStateMachine StateMachine { get; private set; }
        protected virtual void InitializeStates() { }

        #endregion

        #region w/ Unity Callback Functioins

        protected virtual void Awake()
        {
            PlayerHolder = GetComponentInParent<PlayerHolder>();
            
            Animator = GetComponent<Animator>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            
            InputHandler = GetComponentInParent<PlayerInputHandler>();
            // Core = transform.parent.GetComponentInChildren<Core>();
            Core = GetComponentInChildren<Core>();

            StateMachine = new PlayerStateMachine();

            // 初始狀態
            // InitializeStates();
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {
            Core.LogicUpdate();
            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
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

        #region w/ Animation Trigger Functions

        private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
        private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
        private void AnimationMinimumRequirementMeetTrigger() => StateMachine.CurrentState.AnimationMinimumRequirementMeetTrigger();

        #endregion
    }
}
