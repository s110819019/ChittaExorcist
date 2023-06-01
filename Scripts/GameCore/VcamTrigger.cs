using System;
using Cinemachine;
using UnityEngine;

namespace ChittaExorcist.GameCore
{
    public class VcamTrigger : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera vcam;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                vcam.enabled = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                vcam.enabled = false;
            }
        }
    }
}