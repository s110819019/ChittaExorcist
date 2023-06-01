using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChittaExorcist.UISettings
{
    public class ButtonPressedBehaviour : StateMachineBehaviour
    {
        public static Dictionary<string, System.Action> buttonFunctionTable;

        private void Awake()
        {
            buttonFunctionTable = new Dictionary<string, System.Action>();
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            UIInput.Instance.DisableAllUIInputs();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!buttonFunctionTable.ContainsKey(animator.gameObject.name))
            {
                // Debug.LogWarning($"{animator.gameObject.name} does not have exit event to invoke");
            }
            else
            {
                buttonFunctionTable[animator.gameObject.name].Invoke();
            }

        }
    }
}
