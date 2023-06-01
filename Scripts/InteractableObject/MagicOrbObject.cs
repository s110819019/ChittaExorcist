using System;
using ChittaExorcist.Common.Variables;
using ChittaExorcist.EventChannel;
using ChittaExorcist.PlayerEffectSettings;
using UnityEngine;

namespace ChittaExorcist.InteractableObject
{
    public class MagicOrbObject : MonoBehaviour
    {
        [SerializeField] private PlayerEffect playerChargeDoneEffect;   
        [SerializeField] private FloatEventChannel onPlayerManaChange;
        [SerializeField] private FloatReference maxMana;
        [SerializeField, Header("Player Mana")] private FloatReference mana;
        // [SerializeField] private FloatReference playerMana;
        
        public void ResetMana()
        {
            if (mana.Value != maxMana.Value)
            {
                mana.Variable.SetValue(maxMana);
                onPlayerManaChange.Broadcast(mana.Value);
                PlayerPlayerChargeDoneEffectEnd();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                ResetMana();
            }
        }
        
        public void PlayerPlayerChargeDoneEffectEnd()
        {
            playerChargeDoneEffect.PlayAnimationFromZero("End");
        }
    }
}