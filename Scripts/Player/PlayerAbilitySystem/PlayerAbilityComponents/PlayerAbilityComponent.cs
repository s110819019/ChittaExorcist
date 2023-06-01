using UnityEngine;

using ChittaExorcist.CharacterCore;
using ChittaExorcist.PlayerSettings.InputHandler;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    public abstract class PlayerAbilityComponent : MonoBehaviour
    {
        #region w/ Events

        // 避免之後與其他 Ability Component 有 Awake 先後問題
        // 改在 Start 和 OnDestroy 做事件訂閱處理
        protected virtual void SetSubscribeEvents()
        {
            Ability.OnEnter += HandleEnter;
            Ability.OnExit += HandleExit;
        }

        protected virtual void SetUnsubscribeEvents()
        {
            Ability.OnEnter -= HandleEnter;
            Ability.OnExit -= HandleExit;
        }

        #endregion
        
        #region w/ Components

        protected PlayerAbility Ability; // 在父層的 Player Ability
        protected AnimationEventHandler EventHandler => Ability.EventHandler;
        protected Core Core => Ability.Core;
        protected PlayerInputHandler InputHandler => Ability.InputHandler;
        protected PlayerAbilityEventChannelHolder AbilityEventChannelHolder;

        #endregion

        #region w/ Variables

        protected bool IsPhaseActive;

        protected float StartTime;

        protected float Duration => Time.time - StartTime;

        protected bool CheckIsFirstAttack => Ability.IsFirstAttack;

        protected void SetIsFirstAttackFalse()
        {
            Ability.IsFirstAttack = false;
        }

        #endregion

        #region w/ Ability Data Settings

        public virtual void InitializePlayerAbilityData() { } // Ability Generator 呼叫

        #endregion
        
        #region w/ Workflow

        protected virtual void HandleEnter()
        {
            IsPhaseActive = true;
            StartTime = Time.time;
        }

        protected virtual void HandleExit()
        {
            IsPhaseActive = false;
        }

        #endregion
        
        #region w/ Core Components
        
        // Player Mana
        private PlayerManaStats PlayerManaStats => _playerManaStats ? _playerManaStats : Core.GetCoreComponent(out _playerManaStats);
        private PlayerManaStats _playerManaStats;
        
        #endregion
        
        #region w/ Unity Callback Functions

        protected virtual void Awake()
        {
            Ability = GetComponent<PlayerAbility>();

            TryGetComponent(out AbilityEventChannelHolder);

            // 因 Ability 目前必定會在 ability component 之前執行 awake, 在此不用注意 ability 與 ability component 的 awake 先後問題
            // EventHandler = GetComponentInChildren<AnimationEventHandler>();
            // Core = Ability.Core;
        }

        protected  virtual void Start()
        {
            SetSubscribeEvents();
            // Debug.Log("AbilityComp sub Event when start : " + transform.parent.transform.parent.name);
        }

        protected virtual void OnDestroy()
        {
            SetUnsubscribeEvents();
        }

        protected virtual bool CheckManaRequire()
        {

            if (!Ability.IsCostMana || CheckIsFirstAttack)
            {
                SetIsFirstAttackFalse();
                return true;
            }
            
            if (PlayerManaStats.CheckManaCost(Ability.ManaRequireCost()))
            {
                PlayerManaStats.DecreaseMana(Ability.ManaRequireCost());
                return true;
            }
            else
            {
                HandleExit();
                return false;
            }
        }

        protected virtual void CheckManaRecover()
        {
            if (!Ability.IsRecoverMana)
            {
                return;
            }
            
            PlayerManaStats.IncreaseMana(Ability.ManaRequireRecover());
        }

        #endregion
    }

    public abstract class PlayerAbilityComponent<T1, T2> : PlayerAbilityComponent
        where T1 : PlayerAbilityComponentData<T2> where T2 : PlayerAbilityPhaseData
    {
        protected T1 ComponentData;

        private T2 _currentPhaseData;
        
        protected T2 CurrentPhaseData
        {
            get => _currentPhaseData ?? ComponentData.PhaseData[Ability.CurrentPhaseCounter];
            // get
            // {
            //     if (_currentPhaseData == null)
            //     {
            //         Debug.Log("Null When Try Get Phase Data");
            //         return ComponentData.PhaseData[Ability.CurrentPhaseCounter];
            //     }
            //     return _currentPhaseData;
            // }
            private set => _currentPhaseData = value;
        }

        protected override void HandleEnter()
        {
            base.HandleEnter();

            CurrentPhaseData = ComponentData.PhaseData[Ability.CurrentPhaseCounter];
            // Debug.Log("Set Current Phase Data");
        }

        public override void InitializePlayerAbilityData()
        {
            base.InitializePlayerAbilityData();

            ComponentData = Ability.Data.GetData<T1>();
            // Debug.Log("Set Component Data");
        }
    }
}