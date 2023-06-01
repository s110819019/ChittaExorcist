using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public abstract class PlayerAbilityComponentData
    {
        [SerializeField, HideInInspector] private string elementName;
        
        // 依賴的 ability component
        public Type ComponentDependency { get; protected set; }

        #region w/ Construtor

        protected PlayerAbilityComponentData()
        {
            SetComponentName();
            SetComponentDependency();
        }        

        #endregion

        protected abstract void SetComponentDependency();

        #region w/ Set Component Data Element Name

        // public void SetComponentName() => elementName = GetType().Name;
        public void SetComponentName()
        {
            var tempName = GetType().Name;
            // 刪除 Ability & Data 等字串
            // e.g. AbilityMovementData
            elementName = tempName.Substring(7, tempName.Length - 11);
        }

        #endregion

        #region w/ Phase Settings

        public virtual void SetPhaseDataName() { }
        public virtual void InitializePhaseData(int numberOfPhases) { }        

        #endregion
    }

    [Serializable]
    public abstract class PlayerAbilityComponentData<T> : PlayerAbilityComponentData where T : PlayerAbilityPhaseData
    {
        // 所有的 Ability Phase Data
        [SerializeField] private T[] phaseData;

        public T[] PhaseData
        {
            get => phaseData;
            private set => phaseData = value;
        }

        #region w/ Set Phase Data Element Name

        public override void SetPhaseDataName()
        {
            base.SetPhaseDataName();

            // e.g. Phase 1
            for (var i = 0; i < PhaseData.Length; i++)
            {
                PhaseData[i].SetPhaseName(i + 1);
            }
        }        

        #endregion

        #region w/ Phase Settings

        // 在 Player Ability Data SO Editor 中 呼叫
        // 用來設定 對應 NumberOfPhases 數量的 Phase Data
        public override void InitializePhaseData(int numberOfPhases)
        {
            base.InitializePhaseData(numberOfPhases);

            // 是否已有設定非零個的 Phase Data
            // var oldLength = phaseData != null ? phaseData.Length : 0;
            var oldLength = phaseData?.Length ?? 0;

            // Phase Data 的數量已設定正確
            if (oldLength == numberOfPhases)
            {
                return;
            }
            
            // Phase Data 的數量目前設定不正確(大於 or 小於)
            // 重新設定 Phase Data 大小
            Array.Resize(ref phaseData, numberOfPhases);

            if (oldLength < numberOfPhases)
            {
                for (var i = oldLength; i < phaseData.Length; i++)
                {
                    var newObj = Activator.CreateInstance(typeof(T)) as T;
                    phaseData[i] = newObj;
                }
            }
            
            SetPhaseDataName();
        }        

        #endregion
    }
}