using UnityEngine;

namespace ChittaExorcist.Structs
{
    public struct ParriedDetails
    {
        // Knockback
        public bool IsSetParriedKnockback;
        public ParriedKnockbackDetails ParriedKnockbackDetails;

        // Projectile
        public bool IsSetParriedProjectile;
        public ParriedProjectileDetails ParriedProjectileDetails;
    }
}