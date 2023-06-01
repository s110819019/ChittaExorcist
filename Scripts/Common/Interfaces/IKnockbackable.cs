using UnityEngine;

namespace ChittaExorcist.Common.Interfaces
{
    public interface IKnockbackable
    {
        void Knockback(Vector2 angle, float strength);
        void Knockback(Vector2 angle, float strength, int direction);

        void Knockback(Vector2 angle, float strength, int direction, IParryable parryable);
    }
}