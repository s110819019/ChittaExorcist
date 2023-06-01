
using UnityEngine;

namespace ChittaExorcist.Common.Interfaces
{
    public interface IDamageable
    {
        void Damage(float amount);

        void Damage(float amount, IParryable parryable);
    }
}