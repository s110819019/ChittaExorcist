using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    /// <summary>
    /// 玩家角色狀態機
    /// </summary>
    public class PlayerStateMachine
    {
        // 目前 state
        public PlayerState CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }
        private PlayerState _currentState;

        public void Initialize(PlayerState startState)
        {
            CurrentState = startState;
            CurrentState.Enter();
        }

        public void ChangeState(PlayerState newState)
        {
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }
            else
            {
                Debug.Log("Exit a null state !");
            }
            CurrentState = newState;
            CurrentState.Enter();
        }

        // private void ChangeStateInactive(Player targetPlayer, PlayerStateMachine stateMachine, PlayerState newState)
        // {
        //     if (stateMachine.CurrentState != null)
        //     {
        //         stateMachine.CurrentState.Exit();
        //     }
        //     else
        //     {
        //         Debug.Log("Exit a null state !");
        //     }
        //     stateMachine.CurrentState = newState;
        //     stateMachine.CurrentState.Enter();
        //     targetPlayer.gameObject.SetActive(false);
        // }
        //
        // private void ChangeStateActive(Player targetPlayer, PlayerStateMachine stateMachine, PlayerState newState)
        // {
        //     targetPlayer.gameObject.SetActive(true);
        //     if (stateMachine.CurrentState != null)
        //     {
        //         stateMachine.CurrentState.Exit();
        //     }
        //     else
        //     {
        //         Debug.Log("Exit a null state !");
        //     }
        //     stateMachine.CurrentState = newState;
        //     stateMachine.CurrentState.Enter();
        // }

        public void ChangePlayerAndState(Player currentPlayer, Player newPlayer, PlayerState endState, PlayerState newState)
        {
            ChangeState(endState);
            currentPlayer.gameObject.SetActive(false);
            // ChangeStateInactive(currentPlayer, currentPlayer.StateMachine, endState);

            newPlayer.gameObject.SetActive(true);
            newPlayer.StateMachine.ChangeState(newState);
            // ChangeStateActive(newPlayer, newPlayer.StateMachine, newState);
            
            newPlayer.PlayerHolder.onPlayerSwitch.Broadcast(newPlayer.GetType() == typeof(ChuXiaoPlayer) ? 0 : 1);
        }
    }
}