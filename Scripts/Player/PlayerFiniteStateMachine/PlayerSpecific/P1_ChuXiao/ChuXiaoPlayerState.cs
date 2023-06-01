using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public abstract class ChuXiaoPlayerState<T1> : PlayerState<T1, ChuXiaoPlayer> where T1 : PlayerStateData
    {
        #region w/ Constructor

        protected ChuXiaoPlayerState(string animationBoolName, ChuXiaoPlayer player) : base(animationBoolName, player)
        {
        }
        
        #endregion

        // public override void LogicUpdate()
        // {
        //     base.LogicUpdate();
        //
        //     if (Player.IsOnHit)
        //     {
        //         // Debug.Log(this);
        //         StateMachine.ChangeState(Player.HitState);
        //     }
        // }
    }
}