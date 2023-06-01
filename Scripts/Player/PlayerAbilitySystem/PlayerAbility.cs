using System;
using System.Collections.Generic;
using UnityEngine;

using ChittaExorcist.Common.Utilities;
using ChittaExorcist.CharacterCore;
using ChittaExorcist.Common.Variables;
using ChittaExorcist.PlayerSettings.InputHandler;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    public class PlayerAbility : MonoBehaviour
    {
        // Phase 重置時間CD
        [SerializeField] private float phaseCounterResetCooldown;

        public bool IsFirstAttack;

        public void SetIsFirstAttack()
        {
            IsFirstAttack = true;
        }
        
        #region w/ Events

        public event Action OnEnter;
        public event Action OnExit;
        public event Action OnEarlyOut;

        private void SetSubscribeEvents()
        {
            EventHandler.OnFinish += Exit;
            EventHandler.OnEarlyOut += EarlyOut;
            _phaseCounterResetTimer.OnTimeDone += ResetPhaseCounter;
        }

        private void SetUnsubscribeEvents()
        {
            EventHandler.OnFinish -= Exit;
            EventHandler.OnEarlyOut -= EarlyOut;
            _phaseCounterResetTimer.OnTimeDone -= ResetPhaseCounter;
        }
        
        #endregion

        #region w/ Components
        
        private Animator _animator; // BaseGameObject 的 animator
        private GameObject BaseGameObject { get; set; } // child game object
        
        private Timer _phaseCounterResetTimer; // Phase 重置時間用的計時器
        public AnimationEventHandler EventHandler { get; private set; } // 動畫事件處理

        public Animator Animator
        {
            get => _animator;
            private set => _animator = value;
        }

        private SpriteRenderer _spriteRenderer;

        public SpriteRenderer SpriteRenderer
        {
            get => _spriteRenderer;
            private set => _spriteRenderer = value;
        }
        
        #endregion

        #region w/ Mana

        [field: SerializeField, Header("Recover Mana")] public bool IsRecoverMana { get; private set; }
        [field: SerializeField] public List<FloatReference> ManaRecoverList { get; private set; }
        [field: SerializeField, Header("Cost Mana")] public bool IsCostMana { get; private set; }
        [field: SerializeField] public List<FloatReference> ManaCostList { get; private set; }

        public float ManaRequireRecover()
        {
            return ManaRecoverList[CurrentPhaseCounter].Value;
        }
        
        public float ManaRequireCost()
        {
            return ManaCostList[CurrentPhaseCounter].Value;
        }

        #endregion

        private PlayerMaterialManager _playerMaterialManager;
        
        #region w/ Input Handler

        public PlayerInputHandler InputHandler { get; private set; }

        public void InitializePlayerInputHandler(PlayerInputHandler inputHandler)
        {
            InputHandler = inputHandler;
        }

        #endregion

        #region w/ Core Settings

        public Core Core { get; private set; } // Player 的 Core

        public void InitializeCore(Core core) // Player Awake 呼叫
        {
            Core = core;
            Core.GetCoreComponent(out _playerMaterialManager);
        }

        #endregion

        #region w/ Phase Counter Settings

        private int _currentPhaseCounter; // 計算目前 Phase
        
        public int CurrentPhaseCounter
        {
            get => _currentPhaseCounter;
            private set => _currentPhaseCounter = value >= Data.NumberOfPhases ? 0 : value;
        }
        
        private void ResetPhaseCounter()
        {
            CurrentPhaseCounter = 0;
        }

        #endregion

        #region w/ Ability Data Settings

        public PlayerAbilityDataSO Data { get; private set; }
        
        public void InitializePlayerAbilityData(PlayerAbilityDataSO data) // Ability Generator 呼叫
        {
            Data = data;
        }

        #endregion
        
        #region w/ Workflow

        public void Enter()
        {
            _phaseCounterResetTimer.StopTimer(); // 停止計時, 因已經進入 ability

            _animator.SetBool("active", true);
            _animator.SetInteger("counter", _currentPhaseCounter);

            _playerMaterialManager.InitializeSpriteRenderer(SpriteRenderer);
            
            OnEnter?.Invoke();
        }

        public void Exit()
        {
            // TODO: 似乎有動畫 Bug ?
            _animator.SetBool("active", false);

            CurrentPhaseCounter++;
            
            _phaseCounterResetTimer.StartTimer();
            
            // Debug.Log("Ability Exit");
            OnExit?.Invoke();
        }

        private void EarlyOut()
        {
            OnEarlyOut?.Invoke();
        }
        
        #endregion

        #region w/ Unity Callback Functions

        private void Awake()
        {
            BaseGameObject = transform.Find("Base").gameObject;

            _animator = BaseGameObject.GetComponent<Animator>();

            EventHandler = BaseGameObject.GetComponent<AnimationEventHandler>();

            _spriteRenderer = BaseGameObject.GetComponent<SpriteRenderer>();

            _phaseCounterResetTimer = new Timer(phaseCounterResetCooldown);
        }

        private void Update()
        {
            _phaseCounterResetTimer.Tick();
        }

        private void OnEnable()
        {
            SetSubscribeEvents();
        }

        private void OnDisable()
        {
            SetUnsubscribeEvents();
        }

        #endregion
        
    }
}