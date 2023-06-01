using ChittaExorcist.Structs;
using UnityEngine;

namespace ChittaExorcist.Common.Interfaces
{
    public interface IParryable
    {
        public bool IsParried { get; }
        public Transform AttackTransform { get; set; }
        public void Parry();

        public void CheckParryDetails(ParriedDetails parriedDetails);

        public bool IsSceneTrap { get; }
        
        // public void CheckParry(KnockbackDetails knockbackDetails);

        // public void CheckParry
        // public void CheckParry(ProjectileGetParriedDetails getParriedDetails);
    }
}