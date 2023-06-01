using ChittaExorcist.GameCore;
using UnityEngine;

namespace ChittaExorcist.CharacterCore
{
    public class ParticleManager : CoreComponent
    {
        #region w/ Core Components

        // Movement
        private Movement _movement;
        private Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);

        #endregion

        #region w/ Particles

        public void GetParticle(GameObject particle, Vector2 particleOffset)
        {
            var targetParticle = ObjectPoolManager.Instance.GetObject(particle);

            particleOffset.x *= Movement.FacingDirection;

            targetParticle.transform.position = transform.position + (Vector3) particleOffset;

            if (Movement.FacingDirection == -1)
            {
                targetParticle.transform.Rotate(0.0f, 180.0f, 0.0f);
            }
        }
        
        public void GetParticle(GameObject particle, Vector2 particleOffset, Transform spawnTransform)
        {
            var targetParticle = ObjectPoolManager.Instance.GetObject(particle);

            particleOffset.x *= Movement.FacingDirection;

            targetParticle.transform.position = spawnTransform.position + (Vector3) particleOffset;

            if (Movement.FacingDirection == -1)
            {
                targetParticle.transform.Rotate(0.0f, 180.0f, 0.0f);
            }
        }

        public void GetParticleWithRotate(GameObject particle, Vector2 particleOffset, Transform spawnTransform, float startRotationZ, float endRotationZ)
        {
            var targetParticle = ObjectPoolManager.Instance.GetObject(particle);

            particleOffset.x *= Movement.FacingDirection;

            targetParticle.transform.position = spawnTransform.position + (Vector3) particleOffset;
            
            startRotationZ = Mathf.Clamp(startRotationZ, -360.0f, 360.0f);
            endRotationZ = Mathf.Clamp(endRotationZ, -360.0f, 360.0f);
            var rotationZ = Random.Range(startRotationZ, endRotationZ);
            
            targetParticle.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
            
            if (Movement.FacingDirection == -1)
            {
                targetParticle.transform.Rotate(0.0f, 180.0f, 0.0f);
            }
        }

        #endregion
    }
}