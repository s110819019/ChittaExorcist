using System.Collections;
using System.Collections.Generic;
using ChittaExorcist.Common.Interfaces;
using ChittaExorcist.EnemySettings.FSM;
using ChittaExorcist.Structs;
using UnityEngine;

namespace ChittaExorcist
{
    public class SpikeTrap : MonoBehaviour
    {
    [Header("Spike Info")]
    [SerializeField] private float damageAmount = 20.0f;
    [SerializeField] private Vector2 knockbackAngle = new Vector2(1, 2);
    [SerializeField] private float knockbackStrength = 10.0f;
    
    
    #region w/ Components

    private BoxCollider2D _boxCollider;
    private TrapParryableAttack _parryableAttack;

    #endregion

    #region w/ On Trigger
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCombat"))
        {
            // TODO: 只會對玩家造成傷害?
            // Damage
            IDamageable damageable = other.GetComponent<IDamageable>();
            damageable?.Damage(damageAmount, _parryableAttack);
            
            // Knockback
            IKnockbackable knockbackable = other.GetComponent<IKnockbackable>();
            knockbackable?.Knockback(knockbackAngle, knockbackStrength);
        }
    }
    
    #endregion
    
    #region w/ Unity Callback Functions

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        TryGetComponent(out _parryableAttack);
        if (_parryableAttack != null)
        {
            _parryableAttack.IsSceneTrap = true;
        }
        else
        {
            Debug.LogWarning("Spike Trap no enemy parryable attack on it");
        }
    }

    #endregion
    }
}
