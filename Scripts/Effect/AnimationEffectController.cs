using System;
using UnityEngine;

using ChittaExorcist.Common;
using ChittaExorcist.GameCore;

namespace ChittaExorcist.Effect
{
    public class AnimationEffectController : MonoBehaviour
    {
        private bool _isAlive;
        
        private ParticleSystem _particle;

        private void Awake()
        {
            TryGetComponent(out _particle);
        }

        private void OnEnable()
        {
            _isAlive = true;
            if (_particle != null)
            {
                _particle.Play();
            }
        }

        private void OnDisable()
        {
            _isAlive = false;
            // if (_particle != null)
            // {
            //     _particle.Stop();
            // }
        }

        private void Update()
        {
            if (_particle == null)
            {
                return;
            }

            if (!_isAlive)
            {
                return;
            }
            
            if (!_particle.IsAlive())
            {
                ObjectPoolManager.Instance.ReturnObject(gameObject);
            }
        }

        private void FinishAnimation()
        {
            gameObject.transform.rotation = Quaternion.identity; // 重設旋轉
            ObjectPoolManager.Instance.ReturnObject(gameObject);
        }
    }
}