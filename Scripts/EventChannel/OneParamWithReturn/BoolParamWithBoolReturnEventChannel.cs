using UnityEngine;

namespace ChittaExorcist.EventChannel
{
    [CreateAssetMenu(fileName = "NewBoolParamBoolReturnEventChannel", 
        menuName = "Custom Data/Event Channel/One Param With Return Event Channel/Bool Param With Bool Return Event Channel")]
    public class BoolParamWithBoolReturnEventChannel : EventChannelWithOneParameterAndReturn<bool, bool>
    {
        
    }
}