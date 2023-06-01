using System;
using ChittaExorcist.CharacterCore;
using ChittaExorcist.Common.Interfaces;
using ChittaExorcist.Structs;
using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class EnemyParryableAttack : MonoBehaviour, IParryable
    {
        #region w/ Parryable Interface

        public bool IsParried { get; private set; }
        public bool IsSceneTrap { get; set; }
        public Transform AttackTransform { get; set; }

        public void Parry()
        {
            IsParried = true;
            // Debug.Log("Parried");
        }

        public void CheckParryDetails(ParriedDetails parriedDetails)
        {
            // _enemy.SetParryKnockbakeInfo(knockbackDetails);
            _enemy.SetParriedDetails(parriedDetails);
        }

        #endregion

        public void ResetParryCheck()
        {
            IsParried = false;
        }

        public void SetAttackTransform(Transform attackTransform)
        {
            AttackTransform = attackTransform;
        }
        
        
        private Enemy _enemy;
        private KnockbackReceiver _knockbackReceiver;

        #region w/ Unity Callback Function

        private void Awake()
        {
            transform.parent.TryGetComponent(out _enemy);
        }        

        #endregion
        

    }
}