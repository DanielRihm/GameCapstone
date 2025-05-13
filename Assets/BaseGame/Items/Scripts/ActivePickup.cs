using LCPS.SlipForge;
using LCPS.SlipForge.Weapon;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ActivePickup : MonoBehaviour
{
    public ActiveData Pickup;
    SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = Pickup.Sprite;
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<PlayerInstance>() is PlayerInstance player && player.WeaponRig is WeaponRig rig)
        {
            if (rig.EquipActivePickup(Pickup))
                Destroy(gameObject);
        }
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
