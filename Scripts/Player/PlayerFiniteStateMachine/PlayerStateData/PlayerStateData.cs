using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    [Serializable]
    public class PlayerStateData
    {
        [SerializeField, HideInInspector] private string elementName;

        // public void SetElementName() => elementName = GetType().Name;
        public void SetElementName()
        {
            var tempName = GetType().Name;
            // 刪除 Player & StateData 等字串
            elementName = tempName.Substring(6, tempName.Length - 15);
        }
    }
}