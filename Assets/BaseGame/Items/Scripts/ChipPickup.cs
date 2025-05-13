using LCPS.SlipForge;
using LCPS.SlipForge.Engine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ChipPickup : MonoBehaviour
{
	SpriteRenderer _spriteRenderer;
	public Sprite Sprite;
	public Observable<int> Chips => DataTracker.Instance.SaveData.Currency.PotatoChips;

	[SerializeField] private AudioClip PickupSound;
	[SerializeField] private AudioClip DropSound;

	// Start is called before the first frame update
	void Start()
    {
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_spriteRenderer.sprite = Sprite;

		SoundManager.Instance.RegisterSFX(PickupSound.name, PickupSound);
		SoundManager.Instance.RegisterSFX(DropSound.name, DropSound);

		SoundManager.Instance.PlaySFX(DropSound.name);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<PlayerInstance>() is PlayerInstance player)
		{
			Chips.Value += 10;
			SoundManager.Instance.PlaySFX(PickupSound.name);
			Destroy(this.gameObject);
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
