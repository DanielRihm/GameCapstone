using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LCPS.SlipForge
{
    public class BossDrop : MonoBehaviour
    {
        public AudioClip DeathSound;

        // Start is called before the first frame update
        void Start()
        {
            SoundManager.Instance.RegisterSFX(DeathSound.name, DeathSound);
            SoundManager.Instance.PlaySFX(DeathSound.name);

            StartCoroutine(DestroySelf());
        }

        public IEnumerator DestroySelf()
        {
            yield return new WaitForSeconds(DeathSound.length);

            Destroy(transform.gameObject);
        }
    }
}
