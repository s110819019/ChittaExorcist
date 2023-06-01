using UnityEngine;

namespace ChittaExorcist.EventChannel
{
    [CreateAssetMenu(fileName = "NewTransformEventChannel", menuName = "Custom Data/Event Channel/One Param Event Channel/Transform Event Channel")]
    public class TransformEventChannel : EventChannelWithOneParameter<Transform>
    {
        
    }
}