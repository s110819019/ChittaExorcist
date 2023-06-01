using System;
using UnityEngine;

namespace ChittaExorcist.CharacterCore
{
    public class Death : CoreComponent
    {
        // private DamageReceiver _damageReceiver;
        // private MaterialManager _materialManager;
        // Movement
        private MaterialManager _materialManager;
        private MaterialManager MaterialManager => _materialManager ? _materialManager : Core.GetCoreComponent(out _materialManager);

        #region w/ Events

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();

            MaterialManager.OnDeathOver += OnDeathOver;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            
            MaterialManager.OnDeathOver -= OnDeathOver;
        }

        #endregion
        
        protected override void Start()
        {
            base.Start();
            // Core.GetCoreComponent(out _damageReceiver);
            // Core.GetCoreComponent(out _materialManager);
        }


        private void OnDeathOver()
        {
            Destroy(Core.transform.parent.gameObject);
        }
        
        
    }
}