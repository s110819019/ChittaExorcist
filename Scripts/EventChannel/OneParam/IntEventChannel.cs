using UnityEngine;

namespace ChittaExorcist.EventChannel
{
    [CreateAssetMenu(fileName = "NewIntEventChannel", menuName = "Custom Data/Event Channel/One Param Event Channel/Int Event Channel")]

    public class IntEventChannel : EventChannelWithOneParameter<int>
    {
        
    }
}