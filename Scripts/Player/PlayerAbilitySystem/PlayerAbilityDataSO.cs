using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [CreateAssetMenu(fileName = "NewPlayerAbilityData", menuName = "Custom Data/Player/Player Ability Data SO")]
    public class PlayerAbilityDataSO : ScriptableObject
    {
        [field: SerializeField] public int NumberOfPhases { get; private set; }
        [field: SerializeReference] public List<PlayerAbilityComponentData> ComponentData { get; private set; }

        // 從 List 取得指定類型的 Ability Component Data
        // 由 Player Ability Component 呼叫, 以取得對應的資料
        public T GetData<T>()
        {
            return ComponentData.OfType<T>().FirstOrDefault();
        }

        // 添加 Ability Component Data 到 List
        // 由 Editor Script 呼叫
        public void AddData(PlayerAbilityComponentData componentData)
        {
            if (ComponentData.FirstOrDefault(t => t.GetType() == componentData.GetType()) != null)
            {
                return;
            }
            ComponentData.Add(componentData);
        }

        // 取得依賴的 Ability Component
        public List<Type> GetAllDependencies()
        {
            return ComponentData.Select(component => component.ComponentDependency).ToList();
        }
    }
}