using System;
using UnityEngine;

using ChittaExorcist.PlayerSettings.PlayerAbilitySystem;
using ChittaExorcist.CharacterCore;
using ChittaExorcist.GameCore.AudioSettings;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public class ChuXiaoPlayer : Player
    {
        [SerializeField] private AudioDataSO moveLoopAudio;
        [Header("Next Player")] [SerializeField]
        private ShaoYuePlayer shaoYue;

        public ShaoYuePlayer ShaoYue
        {
            get => shaoYue;
            private set => shaoYue = value;
        }
        
        #region w/ Event Subscribe

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            PlayerDamageReceiver.OnHit += HandleOnHit;
            _playerHealthStats.OnHeathZero += HandleOnDeath;
            PlayerHolder.onPlayerRespawn.AddListener(HandleIdle);
            PlayerHolder.onPlayerFlip.AddListener(FlipPlayer);
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            PlayerDamageReceiver.OnHit -= HandleOnHit;
            _playerHealthStats.OnHeathZero -= HandleOnDeath;
            PlayerHolder.onPlayerRespawn.RemoveListener(HandleIdle);
            PlayerHolder.onPlayerFlip.RemoveListener(FlipPlayer);
        }

        #endregion

        #region w/ State Variables

        // Grounded
        public ChuXiaoIdleState IdleState { get; private set; }
        public ChuXiaoMoveState MoveState { get; private set; }
        public ChuXiaoLandState LandState { get; private set; }
        // Independent
        public ChuXiaoInAirState InAirState { get; private set; }
        // Ability
        public ChuXiaoJumpState JumpState { get; private set; }
        public ChuXiaoDashState DashState { get; private set; }
        public ChuXiaoHitState HitState { get; private set; }
        
        public ChuXiaoDeathState DeathState { get; private set; }
        
        public ChuXiaoChargeState ChargeState { get; private set; }
        
        // Sub Abilities
        public ChuXiaoSubAbilityState LightAttackState { get; private set; }
        public ChuXiaoSubAbilityState AirLightAttackState { get; private set; }
        
        public ChuXiaoSubAbilityState BlockState { get; private set; }

        #endregion

        #region w/ Sub Abilities

        private PlayerAbility _lightAttack;
        private PlayerAbility _airLightAttack;
        private PlayerAbility _block;

        #endregion

        #region w/ Init States

        protected override void InitializeStates()
        {
            base.InitializeStates();
            IdleState = new ChuXiaoIdleState("idle", this);
            MoveState = new ChuXiaoMoveState("move", this);
            LandState = new ChuXiaoLandState("land", this);
            InAirState = new ChuXiaoInAirState("inAir", this);
            JumpState = new ChuXiaoJumpState("inAir", this);
            DashState = new ChuXiaoDashState("dash", this);
            HitState = new ChuXiaoHitState("hit", this);

            DeathState = new ChuXiaoDeathState("death", this);

            ChargeState = new ChuXiaoChargeState("charge", this);
            
            LightAttackState = new ChuXiaoSubAbilityState("ability", this, _lightAttack);
            AirLightAttackState = new ChuXiaoSubAbilityState("ability", this, _airLightAttack);

            BlockState = new ChuXiaoSubAbilityState("ability", this, _block);
        }

        #endregion

        #region w/ Unity Callback Functions

        protected override void Awake()
        {
            base.Awake();
            Character = PlayerCharacter.ChuXiao;

            // TODO: 如何設定背包?
            _lightAttack = transform.Find("AbilitySystem/LightAttack").GetComponent<PlayerAbility>();
            _lightAttack.InitializeCore(Core);
            _lightAttack.InitializePlayerInputHandler(InputHandler);

            _airLightAttack = transform.Find("AbilitySystem/AirLightAttack").GetComponent<PlayerAbility>();
            _airLightAttack.InitializeCore(Core);
            _airLightAttack.InitializePlayerInputHandler(InputHandler);
            
            _block = transform.Find("AbilitySystem/Block").GetComponent<PlayerAbility>();
            _block.InitializeCore(Core);
            _block.InitializePlayerInputHandler(InputHandler);
            
            // TODO: 放哪裡?
            Core.GetCoreComponent(out _playerMaterialManager);
            Core.GetCoreComponent(out _playerHealthStats);

            InitializeStates();
        }

        protected override void Start()
        {
            _playerMaterialManager.InitializeSpriteRenderer(SpriteRenderer);
            StateMachine.Initialize(IdleState);
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        #endregion

        #region w/ Movement Core Comp For Check In Air State

        // Movement
        private Movement _movement;
        private Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);

        // Damage
        private PlayerDamageReceiver PlayerDamageReceiver => _playerDamageReceiver ? _playerDamageReceiver : Core.GetCoreComponent(out _playerDamageReceiver);
        private PlayerDamageReceiver _playerDamageReceiver;

        private PlayerMaterialManager _playerMaterialManager;

        private PlayerHealthStats _playerHealthStats;
        
        #endregion

        #region w/ On Hit

        public bool IsOnHit { get; private set; }
        public void ResetOnHit() => IsOnHit = false;
        private void HandleOnHit()
        {
            if (StateMachine.CurrentState != HitState && StateMachine.CurrentState != DeathState)
            {
                IsOnHit = true;
                StateMachine.ChangeState(HitState);
            }
        }

        #endregion

        #region w/ On Death

        public bool IsOnDeath { get; private set; }
        public void ResetOnDeath() => IsOnDeath = false;

        private void HandleOnDeath()
        {
            // Debug.Log("Death");
            IsOnDeath = true;
            StateMachine.ChangeState(DeathState);
        }

        #endregion

        #region w/ On Idle

        private void HandleIdle()
        {
            StateMachine.ChangeState(IdleState);
        }

        #endregion

        #region w/ On Flip

        private void FlipPlayer()
        {
            Movement.Flip();
        }

        #endregion


        private void OnDestroy()
        {
            AudioManager.Instance.StopAudio(moveLoopAudio);
        }

        #region w/ On GUI

        // private void OnGUI()
        // {
        //     var rect = new Rect(100, 900, 100, 100);
        //     // var message = $"Player State : { StateMachine.CurrentState.GetType().Name }";
        //
        //     var message = "Player State : P1_";
        //     
        //     if (StateMachine.CurrentState == IdleState)
        //     {
        //         message += "Idle";
        //
        //     }
        //     else if (StateMachine.CurrentState == MoveState)
        //     {
        //         message += "Move";
        //     }
        //     else if (StateMachine.CurrentState == LandState)
        //     {
        //         message += "Land";
        //     }
        //     else if (StateMachine.CurrentState == InAirState)
        //     {
        //         if (Movement.CurrentVelocity.y <= 0.01f)
        //         {
        //             message += "InAir_Fall";
        //         }
        //         else
        //         {
        //             message += "InAir_Up";
        //         }
        //     }
        //     else if (StateMachine.CurrentState == JumpState)
        //     {
        //         message += "Jump";
        //     }
        //     else if (StateMachine.CurrentState == DashState)
        //     {
        //         message += "Dash";
        //     }
        //     else if (StateMachine.CurrentState == LightAttackState)
        //     {
        //         message += "Ability_LightAttack";
        //     }
        //     else if (StateMachine.CurrentState == AirLightAttackState)
        //     {
        //         message += "Ability_AirLightAttack";
        //     }
        //     else if (StateMachine.CurrentState == HitState)
        //     {
        //         message += "Hit";
        //     }
        //     else if (StateMachine.CurrentState == ChargeState)
        //     {
        //         message += "Charge";
        //     }
        //     else if (StateMachine.CurrentState == BlockState)
        //     {
        //         message += "Ability_Block";
        //     }
        //     else
        //     {
        //         Debug.Log("Not Record State");
        //     }
        //
        //
        //     var style = new GUIStyle();
        //
        //     style.fontSize = 24;
        //     style.fontStyle = FontStyle.Bold;
        //
        //     // style.normal.textColor = new Color(99.0f / 256.0f, 129.0f / 256.0f, 163.0f / 256.0f);
        //     style.normal.textColor = new Color(250.0f / 256.0f, 250.0f / 256.0f, 250.0f / 256.0f);
        //     GUI.Label(rect, message, style);
        // }

        #endregion
    }
}