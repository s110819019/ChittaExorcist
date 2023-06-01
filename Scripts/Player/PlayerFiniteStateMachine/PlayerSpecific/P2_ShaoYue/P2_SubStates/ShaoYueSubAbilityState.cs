using UnityEngine;

using ChittaExorcist.PlayerSettings.PlayerAbilitySystem;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public class ShaoYueSubAbilityState : ShaoYueAbilityState<PlayerSubAbilityStateData>
    {
        public readonly PlayerAbility Ability;

        public ShaoYueSubAbilityState(string animationBoolName, ShaoYuePlayer player, PlayerAbility ability) : base(animationBoolName, player)
        {
            Ability = ability;
            
            ability.OnExit += ExitHandler;
        }
        
        private void ExitHandler()
        {
            AnimationFinishTrigger();
            IsAbilityDone = true;
            // Debug.Log("Sub Ability State ExitHandler");
        }
        
        #region w/ State Workflow

        public override void Enter()
        {
            base.Enter();

            Movement.SetVelocityZero();
            
            Ability.Enter();
        }

        public override void Exit()
        {
            base.Exit();
            if (!IsAbilityDone)
            {
                Ability.Exit();
            }
        }

        #endregion
    }
}