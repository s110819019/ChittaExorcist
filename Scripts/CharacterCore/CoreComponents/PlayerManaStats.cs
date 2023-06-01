using System;
using UnityEngine;

using ChittaExorcist.Common.Variables;
using ChittaExorcist.EventChannel;

namespace ChittaExorcist.CharacterCore
{
    public class PlayerManaStats : CoreComponent
    {
        [SerializeField] private FloatEventChannel onPlayerManaChange;

        [SerializeField, Header("Player Mana")] private FloatReference mana;
        [SerializeField] private FloatReference maxMana;
        
        #region w/ Core Components

        private CoreComp<DamageReceiver> _damageReceiver;

        #endregion

        #region w/ Mana Decrease Check

        private float _manaFullStartTime;
        private bool _isManaStartFull;
        
        private float _manaLastStartTime;

        private bool _startDecreaseMana;
        
        private void CheckManaShouldSave()
        {
            if (ManaIsFull() && !_isManaStartFull)
            {
                _isManaStartFull = true;
                _manaFullStartTime = Time.time;
            }

            if (_isManaStartFull)
            {
                _startDecreaseMana = false;
                if (Time.time >= _manaFullStartTime + 4.0f)
                {
                    _isManaStartFull = false;
                    _startDecreaseMana = true;
                    DecreaseMana(5.0f);
                }
            }
            else if (_startDecreaseMana)
            {
                if (Time.time >= _manaLastStartTime + 2.0f)
                {
                    _manaLastStartTime = Time.time;
                    DecreaseMana(5.0f);
                }
            }
        }        

        #endregion
        
        #region w/ Mana
        
        public void ResetMana()
        {
            mana.Variable.SetValue(maxMana);
            onPlayerManaChange.Broadcast(mana.Value);
        }
        
        public void DecreaseMana(float decreaseAmount)
        {
            if (mana.Value - decreaseAmount <= 0.0f)
            {
                mana.Variable.SetValue(0.0f);
                onPlayerManaChange.Broadcast(mana.Value);
                // Debug.Log("Player Mana Zero");

            }
            else
            {
                mana.Variable.ApplyChange(-decreaseAmount);
                onPlayerManaChange.Broadcast(mana.Value);
            }

            _isManaStartFull = false;
            _manaLastStartTime = Time.time;
        }
        
        public void IncreaseMana(float increaseAmount)
        {
            if (mana.Value + increaseAmount >= maxMana.Value)
            {
                mana.Variable.SetValue(maxMana.Value);
                onPlayerManaChange.Broadcast(mana.Value);
                // Debug.Log("Player Mana Full");
                
                // FULL
                _isManaStartFull = true;
                _manaFullStartTime = Time.time;
            }
            else
            {
                mana.Variable.ApplyChange(increaseAmount);
                onPlayerManaChange.Broadcast(mana.Value);
            }
            
            _manaLastStartTime = Time.time;
        }

        public bool CheckManaCost(float valueToCost)
        {
            return mana.Value >= valueToCost;
        }

        public bool ManaIsZero()
        {
            return mana.Value <= 0.0f;
        }

        public bool ManaIsFull()
        {
            return mana.Value >= maxMana.Value;
        }

        #endregion
        
        #region w/ Unity Callback Function

        protected override void Awake()
        {
            base.Awake();
            _damageReceiver = new CoreComp<DamageReceiver>(Core);
        }

        protected override void Start()
        {
            base.Start();
            ResetMana();
            onPlayerManaChange.Broadcast(mana.Value);
        }

        private void Update()
        {
            // CheckManaShouldSave();
        }

        #endregion
        
        #region w/ Menu Test
        
        [ContextMenu("Broadcast Player Mana Change Event")]
        private void TestOnPlayerManaChange()
        {
            onPlayerManaChange.Broadcast(mana.Value);
        }
        
        #endregion
    }
}