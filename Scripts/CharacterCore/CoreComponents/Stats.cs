using System;
using UnityEngine;

namespace ChittaExorcist.CharacterCore
{
    public class Stats : CoreComponent
    {
        // [Header("Basic Stats Info")]
        // [SerializeField] private float maxHealth;
        [field: SerializeField] public CoreStat Health { get; private set; }
        
        #region w/ Events

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            // Health.OnCurrentValueZero += OnHealthZero;
            Health.OnCurrentValueZero += OnHealthZeroAction;
            
            _damageReceiver.OnDamage += Health.Decrease;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            // Health.OnCurrentValueZero -= OnHealthZero;
            Health.OnCurrentValueZero -= OnHealthZeroAction;

            _damageReceiver.OnDamage -= Health.Decrease;
        }

        #endregion
        
        #region w/ Core Component
        
        private DamageReceiver _damageReceiver;

        #endregion
        
        #region w/ Health

        public event Action OnHealthZero; // 血量為零的 event

        private void OnHealthZeroAction()
        {
            OnHealthZero?.Invoke();
        }

        #endregion
        
        #region w/ Unity Callback Functions

        protected override void Awake()
        {
            base.Awake();

            Core.GetCoreComponent(out _damageReceiver);

            Health.Init();
        }

        #endregion
    }
}