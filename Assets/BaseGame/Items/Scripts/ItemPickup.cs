using UnityEngine;

namespace LCPS.SlipForge.Weapon
{

    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemPickup : MonoBehaviour
    {
        public WeaponData Pickup;
        SpriteRenderer _spriteRenderer;
        // Start is called before the first frame update
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = Pickup.Sprite;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerInstance>() is PlayerInstance player && player.WeaponRig is WeaponRig rig)
            {
                if(rig.Equip(Pickup))
                    Destroy(gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}