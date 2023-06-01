using UnityEngine;

namespace ChittaExorcist.PlayerEffectSettings
{
    public class PlayerEffect : MonoBehaviour
    {
        private Animator _animator;

        #region w/ Unity Callback Functions

        private void Awake()
        {
            TryGetComponent(out _animator);
        }

        #endregion

        public void PlayAnimationFromZero(string animationName)
        {
            _animator.Play(animationName, -1, 0.0f);
        }

        public void PlayAnimation(string animationName)
        {
            _animator.Play(animationName);
        }
        
        public void PlayEmpty()
        {
            _animator.Play("Empty");
        }
    }
}
