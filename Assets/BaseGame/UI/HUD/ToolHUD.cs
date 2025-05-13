using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LCPS.SlipForge.UI
{
    public abstract class ToolHUD : MonoBehaviour
    {
        [SerializeField] protected Image Icon;
        [SerializeField] protected Image Overlay;
        [SerializeField] protected TextMeshProUGUI AmmoCount;
        protected Sprite _transparentSprite;
        protected float _maxAmmo;

        protected virtual void Start()
        {
            // create a transparent sprite for when the player has no tool equipped
            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, new Color(0, 0, 0, 0));
            texture.Apply();
            _transparentSprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        }

        protected void UpdateAmmoCount(float ammo)
        {
            if (ammo != _maxAmmo)
            {
                AmmoCount.text = Math.Round(ammo, 2).ToString("F2");
                AmmoCount.color = ammo > 0 ? Color.red : Color.white;
            }
            else
            {
                AmmoCount.text = "A";
                AmmoCount.color = Color.green;
            }
            Overlay.fillAmount = ammo / _maxAmmo;
        }

        protected void UpdateAmmoCount(int ammo)
        {
            if (ammo > 0)
            {
                AmmoCount.text = ammo.ToString();
                AmmoCount.color = Color.white;
            }
            else if (ammo == 0)
            { 
                AmmoCount.text = "R";
                AmmoCount.color = Color.red;
            }
            else
            {
                AmmoCount.text = "∞";
                AmmoCount.color = Color.white;
            }
            Overlay.fillAmount = 1 - ammo / _maxAmmo;
        }
    }
}
