
using ChittaExorcist.GameCore;
using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    public class ChuXiaoIdleState : ChuXiaoGroundedState<PlayerIdleStateData>
    {
        public ChuXiaoIdleState(string animationBoolName, ChuXiaoPlayer player) : base(animationBoolName, player)
        {
            // 1. Move
        }

        private int _currentYInput;
        private float _startCameraCheckTime;
        
        #region w/ State Workflow
    
        public override void Enter()
        {
            base.Enter();
            Movement.SetVelocityX(0);
            // Debug.Log("Enter Idle");
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            
            Movement.SetVelocityX(0);

            // if (YInput == 0)
            // {
            //     _currentYInput = 0;
            // }
            //
            // if (YInput == 1 && _currentYInput != 1)
            // {
            //     _currentYInput = 1;
            //     CameraController.Instance.MoveTransposerOffset(YInput);
            // }
            // else if (YInput == -1 && _currentYInput != -1)
            // {
            //     _currentYInput = -1;
            //     CameraController.Instance.MoveTransposerOffset(YInput);
            // }

            // if (YInput == 1)
            // {
            //     AudioManager.instance.PlayAudio(StateData.AudioData);
            // }
            // else if (YInput == -1)
            // {
            //     AudioManager.instance.StopAudio(StateData.AudioData);
            // }
            //
            // if (XInput == 1)
            // {
            //     AudioManager.instance.PlayAudio(StateData.AudioData2);
            // }
            //
            // if (XInput == -1)
            // {
            //     AudioManager.instance.StopAudio(StateData.AudioData2);
            // }

            // if (YInput == -1)
            // {
            //     InputManager.Instance.DisableAllInputs();
            // }

            if (XInput != 0)
            {
                // Move
                StateMachine.ChangeState(Player.MoveState);
            }
        }

        #endregion
    }
}