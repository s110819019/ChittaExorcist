using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityInputHold : PlayerAbilityComponent<AbilityInputHoldData, AbilityInputHoldPhaseData>
    {
        #region w/ Events

        // public event Action OnAttackInputRelease;

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
        }
    
        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
        }
    
        #endregion

        #region w/ Variables
        
        private bool _targetInput;

        #endregion
        
        #region w/ Core Components
        
        
        
        #endregion
    
        #region w/ InputHold

        // private void CheckInputHold(AbilityInputHoldPhaseData data)
        // {
        //     switch (data.TargetInput)
        //     {
        //         case TargetHoldInput.AttackInput:
        //             OnAttackInputRelease?.Invoke();
        //             break;
        //         default:
        //             break;
        //     }
        // }

        #endregion

        #region w/ Workflow

        protected override void HandleEnter()
        {
            base.HandleEnter();
            Ability.Animator.SetBool("hold", true);
        }

        protected override void HandleExit()
        {
            base.HandleExit();
            Ability.Animator.SetBool("hold", false);
            
            switch (ComponentData.TargetInput)
            {
                case AbilityInputHoldData.TargetHoldInput.AttackInput:
                    InputHandler.UseAttackInput();
                    break;
                case AbilityInputHoldData.TargetHoldInput.BlockInput:
                    InputHandler.UseBlockInput();
                    break;
                default:
                    Debug.LogWarning("Why there is no target input on ability input hold");
                    break;
            }
        }

        #endregion
        
        #region w/ Unity Callback Functions

        private void Update()
        {
            // TODO: 按鍵設置?

            switch (ComponentData.TargetInput)
            {
                case AbilityInputHoldData.TargetHoldInput.AttackInput:
                    _targetInput = Ability.InputHandler.AttackInput;
                    // Debug.Log("Attack Hold");
                    break;
                case AbilityInputHoldData.TargetHoldInput.BlockInput:
                    _targetInput = Ability.InputHandler.BlockInput;
                    // Debug.Log("Block Hold");
                    break;
                default:
                    Debug.LogWarning("Why there is no target input on ability input hold");
                    break;
            }

            if (!_targetInput)
            {
                Ability.Animator.SetBool("hold", false);
            }

            if (!IsPhaseActive)
            {
                return;
            }
            
            if (Duration >= CurrentPhaseData.MaxHoldTime)
            {
                Ability.Animator.SetBool("hold", false);
            }
        }

        #endregion
    }
}
