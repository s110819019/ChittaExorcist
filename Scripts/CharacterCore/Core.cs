using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using ChittaExorcist.Common;

namespace ChittaExorcist.CharacterCore
{
    /// <summary>
    /// Core System 角色核心系統
    /// </summary>
    public class Core : MonoBehaviour
    {
        #region w/ Core 
        
        // 存放所有 children 中的 core component
        private readonly List<CoreComponent> _coreComponents = new List<CoreComponent>();

        // 添加 core component 至 list
        private void AddCoreComponent(CoreComponent coreComponent)
        {
            if (!_coreComponents.Contains(coreComponent))
            {
                _coreComponents.Add(coreComponent);
            }
        }
        
        // 從 list 中尋找對應的 core component
        private T GetCoreComponent<T>() where T : CoreComponent
        {
            // 尋找 list 中第一個, 無則繼續
            var coreComponent = _coreComponents.OfType<T>().FirstOrDefault();
            if (coreComponent != null) return coreComponent;

            // 尋找 children 中符合類型的, 無則報錯
            coreComponent = GetComponentInChildren<T>();
            if (coreComponent != null) return coreComponent;

            // 沒找到
            Debug.LogWarning($"在 {transform.parent.name} 找不到 {typeof(T)} 組件");
            return null;
        }
        
        // 外部使用, 從 list 中尋找對應的 core component, 並更改傳入值
        public T GetCoreComponent<T>(out T value) where T : CoreComponent
        {
            value = GetCoreComponent<T>();
            return value;
        }

        #endregion

        #region w/ Workflow
        
        // 所有的 core component 邏輯更新
        public void LogicUpdate()
        {
            foreach (var coreComponent in _coreComponents)
            {
                coreComponent.LogicUpdate();
            }
        }

        #endregion

        #region w/ Unity Callback Functions

        private void Awake()
        {
            // 取得所有 children 的 core component
            var coreComponents = GetComponentsInChildren<CoreComponent>();
            
            // 將 core component 加入到 list
            foreach (var coreComponent in coreComponents)
            {
                AddCoreComponent(coreComponent);
            }
        }

        #endregion
    }
}
