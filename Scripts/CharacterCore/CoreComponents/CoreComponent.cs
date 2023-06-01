using System;
using UnityEngine;

namespace ChittaExorcist.CharacterCore
{
    /// <summary>
    /// Core System 角色核心系統組件
    /// </summary>
    public class CoreComponent : MonoBehaviour
    {
        protected Core Core;

        #region w/ Workflow
    
        public virtual void LogicUpdate() { }
    
        #endregion

        #region w/ Event Subscribe

        protected virtual void SetSubscribeEvents() { }
        protected virtual void SetUnsubscribeEvents() { }

        #endregion
    
        #region w/ Unity Callback Functions

        protected virtual void Awake()
        {
            Core = transform.parent.GetComponent<Core>();
        
            if (Core == null)
            {
                // 沒有設置到 Core 會報錯
                Debug.LogError($" There is no Core on the parent ! : {transform.parent.parent.name}");
            }
        }

        protected virtual void Start() { }

        protected virtual void OnEnable()
        {
            SetSubscribeEvents();
        }

        protected virtual void OnDisable()
        {
            SetUnsubscribeEvents();
        }

        #endregion
    }
}
