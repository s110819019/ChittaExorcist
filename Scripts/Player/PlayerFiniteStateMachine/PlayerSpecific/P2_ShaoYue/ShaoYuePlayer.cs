using UnityEngine;

using ChittaExorcist.PlayerSettings.PlayerAbilitySystem;
using ChittaExorcist.CharacterCore;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public class ShaoYuePlayer : Player
    {
        private bool _hasFirstStart;
        
        [Header("Next Player")] [SerializeField]
        private ChuXiaoPlayer chuXiao;

        public ChuXiaoPlayer ChuXiao
        {
            get => chuXiao;
            private set => chuXiao = value;
        }
        
        #region w/ Event Subscribe

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            PlayerDamageReceiver.OnHit += HandleOnHit;
            PlayerDamageReceiver.OnDamage += HandleOnDamage;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            PlayerDamageReceiver.OnHit -= HandleOnHit;
            PlayerDamageReceiver.OnDamage -= HandleOnDamage;
        }

        #endregion
        
        #region w/ State Variables

        // Grounded
        public ShaoYueIdleState IdleState { get; private set; }
        public ShaoYueMoveState MoveState { get; private set; }
        public ShaoYueLandState LandState { get; private set; }
        
        // Independent
        public ShaoYueInAirState InAirState { get; private set; }
        
        // Ability
        public ShaoYueJumpState JumpState { get; private set; }
        public ShaoYueHitState HitState { get; private set; }
        
        // Sub Abilities
        public ShaoYueSubAbilityState LightAttackState { get; private set; }
        public ShaoYueSubAbilityState LightUpAttackState { get; private set; }
        public ShaoYueSubAbilityState AirLightAttackState { get; private set; }
        public ShaoYueSubAbilityState BlockState { get; private set; }
        
        // TODO: Heavy 大招設定?
        public ShaoYueSubAbilityState HeavyAttackState { get; private set; }

        #endregion
        
        #region w/ Sub Abilities

        private PlayerAbility _lightAttack;
        private PlayerAbility _lightUpAttack;
        private PlayerAbility _airLightAttack;

        private PlayerAbility _block;
        
        private PlayerAbility _heavyAttack;

        #endregion
        
        #region w/ Init States

        protected override void InitializeStates()
        {
            base.InitializeStates();
            IdleState = new ShaoYueIdleState("idle", this);
            MoveState = new ShaoYueMoveState("move", this);
            LandState = new ShaoYueLandState("land", this);
            InAirState = new ShaoYueInAirState("inAir", this);
            JumpState = new ShaoYueJumpState("inAir", this);
            HitState = new ShaoYueHitState("hit", this);

            LightAttackState = new ShaoYueSubAbilityState("ability", this, _lightAttack);
            LightUpAttackState = new ShaoYueSubAbilityState("ability", this, _lightUpAttack);
            AirLightAttackState = new ShaoYueSubAbilityState("ability", this, _airLightAttack);
            BlockState = new ShaoYueSubAbilityState("ability", this, _block);

            HeavyAttackState = new ShaoYueSubAbilityState("ability", this, _heavyAttack);
        }

        #endregion
        
        #region w/ Unity Callback Functions

        protected override void Awake()
        {
            base.Awake();
            Character = PlayerCharacter.ShaoYue;

            // TODO: 如何設定背包? 韶月攻擊設定未完成
            _lightAttack = transform.Find("AbilitySystem/LightAttack").GetComponent<PlayerAbility>();
            _lightAttack.InitializeCore(Core);
            _lightAttack.InitializePlayerInputHandler(InputHandler);
            
            _lightUpAttack = transform.Find("AbilitySystem/LightUpAttack").GetComponent<PlayerAbility>();
            _lightUpAttack.InitializeCore(Core);
            _lightUpAttack.InitializePlayerInputHandler(InputHandler);

            
            _airLightAttack = transform.Find("AbilitySystem/AirLightAttack").GetComponent<PlayerAbility>();
            _airLightAttack.InitializeCore(Core);
            _airLightAttack.InitializePlayerInputHandler(InputHandler);

            _block = transform.Find("AbilitySystem/Block").GetComponent<PlayerAbility>();
            _block.InitializeCore(Core);
            _block.InitializePlayerInputHandler(InputHandler);
            
            _heavyAttack = transform.Find("AbilitySystem/HeavyAttack").GetComponent<PlayerAbility>();
            _heavyAttack.InitializeCore(Core);
            _heavyAttack.InitializePlayerInputHandler(InputHandler);
            
            InitializeStates();
        }

        protected override void Start()
        {
            Core.GetCoreComponent(out _playerMaterialManager);
            _playerMaterialManager.InitializeSpriteRenderer(SpriteRenderer);
            StateMachine.Initialize(IdleState);
            // gameObject.SetActive(false);
        }

        protected override void Update()
        {
            base.Update();
            // TODO: 程序上可能不好
            // 為了讓其他物件執行過 start, 所以才在 Update 做 set active false
            if (!_hasFirstStart)
            {
                gameObject.SetActive(false);
                _hasFirstStart = true;
            }
            // Debug.Log(StateMachine.CurrentState);
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
        protected PlayerDamageReceiver PlayerDamageReceiver => _playerDamageReceiver ? _playerDamageReceiver : Core.GetCoreComponent(out _playerDamageReceiver);
        private PlayerDamageReceiver _playerDamageReceiver;
        
        
        // Player Mana
        protected PlayerManaStats PlayerManaStats => _playerManaStats ? _playerManaStats : Core.GetCoreComponent(out _playerManaStats);
        private PlayerManaStats _playerManaStats;
        
        private PlayerMaterialManager _playerMaterialManager;

        #endregion
        
        #region w/ On Hit

        public bool IsOnHit { get; private set; }
        public void ResetOnHit() => IsOnHit = false;
        private void HandleOnHit()
        {
            if (StateMachine.CurrentState != HitState)
            {
                IsOnHit = true;
                StateMachine.ChangeState(HitState);
            }
        }

        private void HandleOnDamage(float damageAmount)
        {
            PlayerManaStats.DecreaseMana(damageAmount);
        }

        #endregion
        
        #region w/ On GUI

        // private void OnGUI()
        // {
        //     var rect = new Rect(100, 900, 50, 50);
        //     // var message = $"Player State : { StateMachine.CurrentState.GetType().Name}";
        //     
        //     var message = "Player State : P2_";
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
        //     else if (StateMachine.CurrentState == BlockState)
        //     {
        //         message += "Ability_Block";
        //     }
        //     else if (StateMachine.CurrentState == LightAttackState)
        //     {
        //         message += "Ability_LightAttack";
        //     }
        //     else if (StateMachine.CurrentState == HitState)
        //     {
        //         message += "Hit";
        //     }
        //     else if (StateMachine.CurrentState == HeavyAttackState)
        //     {
        //         message += "Ability_HeavyAttack";
        //     }
        //     else
        //     {
        //         Debug.Log("Not Record State");
        //     }
        //     
        //     
        //     
        //     
        //     var style = new GUIStyle();
        //
        //     style.fontSize = 24;
        //     style.fontStyle = FontStyle.Bold;
        //     style.normal.textColor = new Color(250.0f / 256.0f, 250.0f / 256.0f, 250.0f / 256.0f);
        //
        //     GUI.Label(rect, message, style);
        // }

        #endregion
    }
}