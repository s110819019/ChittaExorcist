using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    /// <summary>
    /// 玩家角色持有的 State 資訊
    /// </summary>
    [CreateAssetMenu(fileName = "NewPlayerData", menuName = "Custom Data/Player/Player Data SO")]
    public class PlayerDataSO : ScriptableObject
    {
        // TODO: 待理解 reference
        [field: SerializeReference] public List<PlayerStateData> StateData { get; private set; }

        public T GetData<T>()
        {
            // Debug.Log($"{typeof(T).Name} 正在試著取得");
            if (StateData.OfType<T>().FirstOrDefault() == null)
            {
                Debug.LogWarning($"{typeof(T)} 未添加到 PlayerStateData 中");
            }
            return StateData.OfType<T>().FirstOrDefault();
        }

        public void AddData(PlayerStateData data)
        {
            if (StateData.FirstOrDefault(t=>t.GetType() == data.GetType()) != null)
            {
                return;
            }

            StateData.Add(data);
        }
    }
}