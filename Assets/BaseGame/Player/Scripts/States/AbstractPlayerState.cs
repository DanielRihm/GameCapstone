using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Assertions;

namespace LCPS.SlipForge.Player
{
    [RequireComponent(typeof(PlayerInstance))]
    public class AbstractPlayerState : MonoBehaviour
    {
        public static Vector2 Velocity = Vector2.zero;
        public static float FrictionCoefficient = 1f;
        public static int SpriteTimer = 4;
        protected PlayerSettingsData PlayerData;

        // OnEnable is called when component gets enabled for the player
        protected virtual void OnEnable()
        {
            var playerInstance = FindObjectOfType<PlayerInstance>();
            PlayerData = playerInstance.PlayerData;

			Assert.IsNotNull(PlayerData, $"{name} PlayerSettings is null");
		}

		//Create ActionMap
		protected ActionMap _inputScheme;

		// Start is called before the first frame update
		protected SpriteRenderer PlayerSpriteRen;
		protected virtual void Awake()
		{
			PlayerSpriteRen = gameObject.GetComponentInChildren<SpriteRenderer>();

            //Enable movement
            _inputScheme = new ActionMap();
            _inputScheme.Enable();
            this.enabled = false;
        }
        protected float moveSpeed = 0.05f;
        
        public bool IsMenuOpen => DataTracker.Instance.IsMenuOpen();
        protected virtual void FixedUpdate()
        {
            transform.position += new Vector3(Velocity.x, 0, Velocity.y) * Time.deltaTime;
            Vector2 Modifier = Velocity.normalized;
            Modifier = -Modifier*FrictionCoefficient;
            if (Modifier.magnitude > Velocity.magnitude) Velocity = Vector2.zero;
            else Velocity += Modifier;
        }
        protected virtual void OnDodge()
        {
            if (IsMenuOpen) return; 

            if (!this.enabled) { return; }
            this.enabled = false;
            GetComponent<DodgeState>().enabled = true;
        }

        protected virtual void OnInteract()
        {
            if (!this.enabled) { return; }
        }

		protected virtual void OnAbility()
		{
			if (!this.enabled) { return; }
			if (DataTracker.Instance.ActiveItem != null)
			{
				BroadcastMessage("OnUse");
			}
		}

		protected virtual void OnDrop()
		{
			if (!this.enabled) { return; }
            if (IsMenuOpen) return;
            if (DataTracker.Instance.LeftWeapon.Value != null)
			{
                PlayerInstance.Instance.Drop(DataTracker.Instance.LeftWeapon);
                DataTracker.SetObservableProperty(data => data.SaveData.Weapons.LeftWeapon, null);
			}
			if(DataTracker.Instance.RightWeapon.Value != null)
			{
                PlayerInstance.Instance.Drop(DataTracker.Instance.RightWeapon);
                DataTracker.SetObservableProperty(data => data.SaveData.Weapons.RightWeapon, null);
			}
			if(DataTracker.Instance.ActiveItem != null)
			{
                DataTracker.SetObservableProperty(data => data.SaveData.Weapons.ActiveItem, null);
                //BroadcastMessage("UnequipActiveItem");
            }
        }

		protected virtual void OnAttack1(InputValue value)
		{
			if (!this.enabled) return;

            // We only communite release if the menu is open
            if (!IsMenuOpen || !value.isPressed)
                PlayerInstance.Instance.WeaponRig.OnAttack1(value.isPressed);
        }

		protected virtual void OnAttack2(InputValue value)
		{
			if (!this.enabled) { return; }
            if (IsMenuOpen) return;
            // This enables automatic weapons, it's up to the weapon to handle it.
            PlayerInstance.Instance.WeaponRig.OnAttack2(value.isPressed);
		}

		protected virtual void OnReload(InputValue value)
		{
			if (!this.enabled) { return; }
            if (IsMenuOpen) return;
            PlayerInstance.Instance.WeaponRig.OnReload();
        }
	}

}
