using UnityEngine;
using UnityEngine.InputSystem;

namespace LCPS.SlipForge.Player
{

	public class DodgeState : AbstractPlayerState
	{
        public Sprite[] down;
        public Sprite[] left;
        public Sprite[] up;
        public Sprite[] right;

        private float _spriteTimer;
        private Vector2 _moveInput;
        private int _spriteCounter;

        float counter;
		Vector3 moveVector;
		protected override void OnEnable()
		{
			base.OnEnable();

			counter = 0;
            _spriteTimer = PlayerData.DodgeSeconds/down.Length;
            _spriteCounter = 0;

            _moveInput = _inputScheme.Player.Move.ReadValue<Vector2>();
			moveVector = new Vector3(_moveInput.x, 0, _moveInput.y);
            moveVector.Normalize();
			//get dodge frames from player data so that upgrades work

            SoundManager.Instance.PlaySFX("roll_sound");


            // Hack to make sure we stop shooting when dodging
            PlayerInstance.Instance.WeaponRig.OnAttack1(false);
            PlayerInstance.Instance.WeaponRig.OnAttack2(false);
        }

		// Update is called once per frame
		protected override void FixedUpdate()
		{
            base.FixedUpdate();

            GetComponent<CapsuleCollider>().enabled = false;

			transform.position += moveVector * PlayerData.DodgeSpeed * Time.deltaTime;
			counter += Time.deltaTime;

            if (Mathf.Abs(_moveInput.x) >= Mathf.Abs(_moveInput.y))
			{
				//Moving Right
                if (_moveInput.x > 0)
                {
					SpriteSetter(right);
                }
				//Moving Left
                else
                {
                    SpriteSetter(left);
                }
                //PlayerSpriteRen.sprite = (moveInput.x > 0 ? right : left);
            }
			else
			{
				//Moving Up
				if (_moveInput.y > 0)
				{
                    SpriteSetter(up);
                } 
				//Moving Down
				else
				{
                    SpriteSetter(down);
                }
				//PlayerSpriteRen.sprite = (moveInput.y > 0 ? up : down);
			}

            if (counter >= PlayerData.DodgeSeconds)
            {
                GetComponent<CapsuleCollider>().enabled = true;
                GetComponent<IdleState>().enabled = true;
                this.enabled = false;
            }
        }

        protected override void OnAttack1(InputValue value)
        {
            // Cannot attack1 while dodging
        }

        protected override void OnAttack2(InputValue value)
        {
            //do nothing
        }

        protected override void OnAbility()
        {
            //do nothing
        }


        protected override void OnInteract()
        {
            //do nothing
        }
        protected override void OnDodge()
        {
            //cannot dodge while dodging
        }

        private void SpriteSetter(Sprite[] spriteSet)
        {
            if(_spriteTimer == 0)
            {
                if (_spriteCounter == spriteSet.Length - 1)
                {
                    _spriteCounter = 0;
                }
                else
                {
                    _spriteCounter++;
                    _spriteTimer = PlayerData.DodgeSeconds / down.Length;
                }
            }
            else
            {
                PlayerSpriteRen.sprite = spriteSet[_spriteCounter];
                _spriteTimer = _spriteTimer - 0.01f;
            }
        }
    }

}