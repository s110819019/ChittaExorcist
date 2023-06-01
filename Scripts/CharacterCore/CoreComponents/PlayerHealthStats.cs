using System;
using UnityEngine;

using ChittaExorcist.Common.Variables;
using ChittaExorcist.EventChannel;

namespace ChittaExorcist.CharacterCore
{
    public class PlayerHealthStats : CoreComponent
    {
        [SerializeField, Header("Broadcast Event")] private FloatEventChannel onPlayerHealthChange;

        [SerializeField, Header("Player Health")] private FloatReference health;
        [SerializeField] private FloatReference maxHealth;

        public event Action OnHeathZero;
        
        #region w/ Events

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            _playerDamageReceiver.Comp.OnDamage += DecreaseHealth;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            _playerDamageReceiver.Comp.OnDamage -= DecreaseHealth;
        }

        #endregion
        
        #region w/ Core Components

        private CoreComp<PlayerDamageReceiver> _playerDamageReceiver;

        #endregion
        
        #region w/ Health

        public void ResetHealth()
        {
            health.Variable.SetValue(maxHealth);
            onPlayerHealthChange.Broadcast(health.Value);
        }
        
        public void DecreaseHealth(float decreaseAmount)
        {
            if (health.Value - decreaseAmount <= 0.0f)
            {
                health.Variable.SetValue(0.0f);
                onPlayerHealthChange.Broadcast(health.Value);
                OnHeathZero?.Invoke();
                // Debug.Log("Player Health Zero");
            }
            else
            {
                health.Variable.ApplyChange(-decreaseAmount);
                onPlayerHealthChange.Broadcast(health.Value);
            }
        }
        
        public void IncreaseHealth(float increaseAmount)
        {
            if (health.Value + increaseAmount >= maxHealth.Value)
            {
                health.Variable.SetValue(maxHealth.Value);
                onPlayerHealthChange.Broadcast(health.Value);
                // Debug.Log("Player Health Full");
            }
            else
            {
                health.Variable.ApplyChange(increaseAmount);
                onPlayerHealthChange.Broadcast(health.Value);
            }
        }

        public bool CheckIfHealthFull => health.Value >= maxHealth.Value;

        #endregion
        
        #region w/ Unity Callback Function

        protected override void Awake()
        {
            base.Awake();

            _playerDamageReceiver = new CoreComp<PlayerDamageReceiver>(Core);
        }

        protected override void Start()
        {
            base.Start();
            ResetHealth();
            onPlayerHealthChange.Broadcast(health.Value);
        }

        #endregion
        
        #region w/ Menu Test
        
        [ContextMenu("Broadcast Player Health Change Event")]
        private void TestOnPlayerHealthChange()
        {
            onPlayerHealthChange.Broadcast(health.Value);
        }

        #endregion
    }
}