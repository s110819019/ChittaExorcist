using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChittaExorcist.ProjectileSettings
{
    public class ProjectileAnimationEventHandler : MonoBehaviour
    {
        public event Action OnFinish;

        private void OnAnimationFinishTrigger() => OnFinish?.Invoke();
    }
}
