using ChittaExorcist.CharacterCore;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public class ChuXiaoChargeState : ChuXiaoAbilityState<PlayerChargeStateData>
    {
        public ChuXiaoChargeState(string animationBoolName, ChuXiaoPlayer player) : base(animationBoolName, player)
        {
        }

        #region w/ Charge

        private float _lastHealTime;
        private bool _isMinChargeRequire;

        public bool CanCharge => PlayerManaStats.CheckManaCost(StateData.ManaValueToCost);
        
        #endregion
        
        #region w/ State Workflow

        public override void Enter()
        {
            base.Enter();
            _isMinChargeRequire = false;
        }

        public override void Exit()
        {
            base.Exit();
            Player.PlayerHolder.StopPlayPlayerChargeEffect();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        
            Movement.SetVelocityX(0f);

            if (!ChargeInput || !PlayerManaStats.CheckManaCost(StateData.ManaValueToCost) || PlayerHealthStats.CheckIfHealthFull)
            {
                IsAbilityDone = true;
                // if (PlayerHealthStats.CheckIfHealthFull || !PlayerManaStats.CheckManaCost(StateData.ManaValueToCost))
                // {
                //     Player.PlayerHolder.PlayerPlayerChargeDoneEffectEnd();
                // }
                return;
            }


            // 時間尚未滿足 min charge require
            if (Time.time - StartTime < StateData.MinChargeRequireTime)
            {
                return;
            }
            else if (Time.time - StartTime >= StateData.MinChargeRequireTime && !_isMinChargeRequire)
            {
                _isMinChargeRequire = true;
                _lastHealTime = Time.time;
            }
            
            Player.PlayerHolder.PlayPlayerChargeEffectLoop();

            if (Time.time - _lastHealTime >= StateData.PerHealRequireTime)
            {
                _lastHealTime = Time.time;
                if (!PlayerManaStats.CheckManaCost(StateData.ManaValueToCost)) return;
                PlayerManaStats.DecreaseMana(StateData.ManaValueToCost);
                PlayerHealthStats.IncreaseHealth(StateData.HealthValueToRecover);
                Player.PlayerHolder.PlayerPlayerChargeDoneEffectEnd();
            }



            // if (Time.time >= StartTime + StateData.MaxHitTime)
            // {
            //     Debug.Log("Why Out Hit Time ?");
            //     IsAbilityDone = true;
            // }
        }

        #endregion
    }
}