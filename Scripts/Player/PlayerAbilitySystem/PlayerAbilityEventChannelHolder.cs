using ChittaExorcist.EventChannel;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    public class PlayerAbilityEventChannelHolder : MonoBehaviour
    {
        [SerializeField] public TransformEventChannel onPlayerGetDamagedWithTransform;
        // [SerializeField] public BoolEventChannel onPlayerSuccessBlock;
    }
}