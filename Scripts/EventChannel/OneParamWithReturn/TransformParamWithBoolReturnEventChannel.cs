using UnityEngine;

namespace ChittaExorcist.EventChannel
{
    [CreateAssetMenu(fileName = "NewTransformParamBoolReturnEventChannel", 
        menuName = "Custom Data/Event Channel/One Param With Return Event Channel/Transform Param With Bool Return Event Channel")]
    public class TransformParamWithBoolReturnEventChannel : EventChannelWithOneParameterAndReturn<Transform, bool>
    {
        
    }
}