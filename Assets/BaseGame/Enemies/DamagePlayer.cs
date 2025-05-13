using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;

namespace LCPS.SlipForge
{
    [RequireComponent(typeof(Collider))]
    public class DamagePlayer : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.GetComponent<PlayerInstance>() is PlayerInstance player)
            {
                player.GotHit();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.GetComponent<PlayerInstance>() is PlayerInstance player)
            {
                player.GotHit();
            }
        }
    }
}
