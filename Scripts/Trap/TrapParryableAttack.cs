using System.Collections;
using System.Collections.Generic;
using ChittaExorcist.Common.Interfaces;
using ChittaExorcist.Structs;
using UnityEngine;

namespace ChittaExorcist
{
    public class TrapParryableAttack : MonoBehaviour, IParryable
    {
        public bool IsParried { get; }
        public Transform AttackTransform { get; set; }
        public void Parry()
        {
            // throw new System.NotImplementedException();
        }

        public void CheckParryDetails(ParriedDetails parriedDetails)
        {
            // throw new System.NotImplementedException();
        }

        public bool IsSceneTrap { get; set; }
    }
}
