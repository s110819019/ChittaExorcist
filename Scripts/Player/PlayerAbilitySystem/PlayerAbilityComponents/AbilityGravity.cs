using ChittaExorcist.CharacterCore;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    public class AbilityGravity : PlayerAbilityComponent<AbilityGravityData, AbilityGravityPhaseData>
    {
        #region w/ Events

        protected override void SetSubscribeEvents()
        {
            base.SetSubscribeEvents();
            EventHandler.OnSetGravity += HandleSetGravity;
            EventHandler.OnResetGravity += HandleResetGravity;
        }

        protected override void SetUnsubscribeEvents()
        {
            base.SetUnsubscribeEvents();
            EventHandler.OnSetGravity -= HandleSetGravity;
            EventHandler.OnResetGravity -= HandleResetGravity;
        }

        #endregion
        
        #region w/ Core Components

        // Movement
        private CoreComp<Movement> _movement;

        #endregion

        #region w/ Gravity

        private void HandleSetGravity()
        {
            _movement.Comp.Rigidbody2D.gravityScale = CurrentPhaseData.FloatingGravityScale;
            
            _movement.Comp.SetVelocityZero();
        }

        private void HandleResetGravity()
        {
            _movement.Comp.Rigidbody2D.gravityScale = ComponentData.InitialGravityScale;
        }

        #endregion

        #region w/ Workflow

        protected override void HandleExit()
        {
            base.HandleExit();

            HandleResetGravity();
            // _movement.Comp.Rigidbody2D.gravityScale = ComponentData.InitialGravityScale;
        }

        #endregion
        
        #region w/ Unity Callback Functions

        protected override void Start()
        {
            base.Start();

            _movement = new CoreComp<Movement>(Core);
        }

        #endregion
    }
}