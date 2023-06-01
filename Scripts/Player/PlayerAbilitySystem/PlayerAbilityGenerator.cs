using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    public class PlayerAbilityGenerator : MonoBehaviour
    {
        [SerializeField] private PlayerAbility ability;
        [SerializeField] private PlayerAbilityDataSO data;

        // 已經添加上的 ability components
        private List<PlayerAbilityComponent> _componentsAlreadyOnPlayerAbility = new List<PlayerAbilityComponent>();

        // 需要被添加的 ability components
        private readonly List<PlayerAbilityComponent> _componentsAddedToPlayerAbility = new List<PlayerAbilityComponent>();

        // 需要被添加的 ability component 的所有類型
        private List<Type> _componentDependencies = new List<Type>();

        [ContextMenu("Test Generate Player Ability")]
        private void TestGeneration()
        {
            GeneratePlayerAbility(data);
        }

        private void GeneratePlayerAbility(PlayerAbilityDataSO targetData)
        {
            // 由此設定 ability 的 data (PlayerAbilityDataSO)
            ability.InitializePlayerAbilityData(targetData);
            
            _componentsAlreadyOnPlayerAbility.Clear();
            _componentsAddedToPlayerAbility.Clear();
            _componentDependencies.Clear();

            // 已經添加上的 ability components
            _componentsAlreadyOnPlayerAbility = GetComponents<PlayerAbilityComponent>().ToList();

            // 需要被添加的 ability component 的所有類型
            _componentDependencies = targetData.GetAllDependencies();
            
            foreach (var dependency in _componentDependencies)
            {
                // 需要被添加的 components 清單已經含有此類型的 ability component, 則直接跳到下次迴圈
                if (_componentsAddedToPlayerAbility.FirstOrDefault(component => component.GetType() == dependency))
                {
                    Debug.Log("Continue");
                    continue;
                }

                // 為 null
                var abilityComponent =
                    _componentsAlreadyOnPlayerAbility.FirstOrDefault(component => component.GetType() == dependency);

                if (abilityComponent == null)
                {
                    // 注意會立刻執行 ability component 的 Awake 與 OnEnable
                    abilityComponent = gameObject.AddComponent(dependency) as PlayerAbilityComponent;
                }

                if (abilityComponent != null)
                {
                    // 由此設定 ability component 的 data (PlayerAbilityComponentData)
                    // 此時 ability component 確定已經在 awake 中取得 ability, 以便取得 ability 的 data
                    abilityComponent.InitializePlayerAbilityData();
                }
                else
                {
                    Debug.LogWarning(" 添加到一個 Ability Component 為 null !");
                }
                
                _componentsAddedToPlayerAbility.Add(abilityComponent);
            }

            // 已經添加上的 ability components 除去需要被添加的 ability components
            var componentsToRemove = _componentsAlreadyOnPlayerAbility.Except(_componentsAddedToPlayerAbility);
            
            // 刪除不需要的 ability components
            foreach (var abilityComponent in componentsToRemove)
            {
                Destroy(abilityComponent);
            }
        }

        #region w/ Unity Callback Functions

        private void Start()
        {
            GeneratePlayerAbility(data);
        }

        #endregion
    }
}