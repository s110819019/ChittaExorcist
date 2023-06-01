﻿using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public class ChuXiaoHitState : ChuXiaoAbilityState<PlayerHitStateData>
    {
        public ChuXiaoHitState(string animationBoolName, ChuXiaoPlayer player) : base(animationBoolName, player)
        {
        }

        #region w/ State Workflow

        public override void Enter()
        {
            base.Enter();
            AudioManager.Instance.PlayOnceAudio(StateData.AudioData);
        }

        public override void Exit()
        {
            base.Exit();
            Player.ResetOnHit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        
            Movement.SetVelocityX(0f);
        
            // TODO: 有奇怪 bug 在暫停時會卡在 hit
            if (IsAnimationFinished)
            {
                IsAbilityDone = true;
            }
            else if (Time.time >= StartTime + StateData.MaxHitTime)
            {
                Debug.Log("Why Out Hit Time ?");
                IsAbilityDone = true;
            }
        }

        #endregion
    }
}