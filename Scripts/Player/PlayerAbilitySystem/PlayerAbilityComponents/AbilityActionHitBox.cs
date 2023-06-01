using System;
using UnityEngine;

using ChittaExorcist.CharacterCore;
using ChittaExorcist.GameCore.AudioSettings;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    public class AbilityActionHitBox : PlayerAbilityComponent<AbilityActionHitBoxData, AbilityActionHitBoxPhaseData>
    {
        #region w/ Events

        public event Action<Collider2D[]> OnDetectedCollider2D;

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            EventHandler.OnActionHitBox += HandleActionHitBox;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            EventHandler.OnActionHitBox -= HandleActionHitBox;
        }

        #endregion

        #region w/ Core Components

        private CoreComp<Movement> _movement;

        #endregion

        #region w/ Variables

        private Vector2 _offset;
        private Collider2D[] _detected;

        #endregion

        #region w/ Action Hit Box

        private void HandleActionHitBox()
        {
            _offset.Set(transform.position.x + (CurrentPhaseData.HitBox.center.x * _movement.Comp.FacingDirection),
                transform.position.y + CurrentPhaseData.HitBox.center.y);

            _detected = Physics2D.OverlapBoxAll(_offset, CurrentPhaseData.HitBox.size, 0.0f,
                ComponentData.DetectableLayers);

            // Debug.Log("Action Hit Box");
            
            AudioManager.Instance.PlayOnceAudio(CurrentPhaseData.AudioData);
            
            if (_detected.Length == 0)
            {
                return;
            }
            
            OnDetectedCollider2D?.Invoke(_detected);
        }

        #endregion

        #region w/ Draw Gizmos

        private void OnDrawGizmosSelected()
        {
            if (ComponentData == null) return;

            foreach (var item in ComponentData.PhaseData)
            {
                if (!item.debug)
                {
                    continue;
                }

                // Gizmos.DrawWireCube(transform.position + (Vector3) item.HitBox.center, item.HitBox.size);
                
                // Gizmos.color = Color.red;
                // Gizmos.DrawWireCube(transform.position + (Vector3) item.HitBox.position, item.HitBox.size);
                
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(transform.position + (Vector3) item.HitBox.center, item.HitBox.size);
            }
        }

        #endregion
        
        #region w/ Unity Callback Functions

        protected override void Start()
        {
            base.Start();
            _movement = new CoreComp<Movement>(Core);
        }

        #endregion
    }
}