using UnityEngine;
using UnityEngine.Audio;

namespace ChittaExorcist.GameCore.AudioSettings
{
    [CreateAssetMenu(fileName = "NewAudioData", menuName = "Custom Data/Audio/Audio Data SO")]
    public class AudioDataSO : ScriptableObject
    {
        [field: Header("Basic Settings"), SerializeField] public AudioClip AudioClip { get; private set; }
        [field: SerializeField] public string AudioName { get; private set; }
        [field: SerializeField, Range(0.0f, 1.0f)] public float AudioVolume { get; private set; } = 1.0f;
        [field: SerializeField] public bool IsLoop { get; private set; }
        [field: SerializeField] public AudioMixerGroup AudioMixerGroup { get; private set; }

        // [field: Header("Fade"), SerializeField] public bool IsFadeIn { get; private set; }
        // [field: SerializeField] public float FadeInDuration { get; private set; }
        // [field: SerializeField] public bool IsFadeOut { get; private set; }
        // [field: SerializeField] public float FadeOutDuration { get; private set; }

        [field: Header("Delay"), SerializeField] public float Delay { get; private set; }
    }
}