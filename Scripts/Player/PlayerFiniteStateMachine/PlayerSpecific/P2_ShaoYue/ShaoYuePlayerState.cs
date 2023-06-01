using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public class ShaoYuePlayerState<T1> : PlayerState<T1, ShaoYuePlayer> where T1 : PlayerStateData
    {
        #region w/ Constructor

        public ShaoYuePlayerState(string animationBoolName, ShaoYuePlayer player) : base(animationBoolName, player)
        {
        }        

        #endregion
    }
}