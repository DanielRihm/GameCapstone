using UnityEngine;

namespace LCPS.SlipForge.Player
{

	public class WalkState : AbstractPlayerState
	{
		public Sprite[] down;
		public Sprite[] left;
		public Sprite[] up;
		public Sprite[] right;

		private int _spriteTimer = SpriteTimer;
		private int _currentSprite = 0;

		// Update is called once per frame
		protected override void FixedUpdate()
		{
            base.FixedUpdate();
            //Read in movement vector values
            Vector2 moveInput = _inputScheme.Player.Move.ReadValue<Vector2>();
			if (Velocity.magnitude <= (moveInput*PlayerData.WalkSpeed).magnitude)
			{
                Velocity = moveInput * PlayerData.WalkSpeed;
            }
			
			if (moveInput.magnitude <= 0)
			{
				GetComponent<IdleState>().enabled = true;
				this.enabled = false;
			}
			else if (Mathf.Abs(moveInput.x) >= Mathf.Abs(moveInput.y))
			{
				//Moving Right
                if (moveInput.x > 0)
                {
					SpriteSetter(PlayerInstance.Instance.Facing == DirectionEnum.Direction.East, right);
					PlayerInstance.Instance.Facing = DirectionEnum.Direction.East;
                }
				//Moving Left
                else
                {
                    SpriteSetter(PlayerInstance.Instance.Facing == DirectionEnum.Direction.West, left);
                    PlayerInstance.Instance.Facing = DirectionEnum.Direction.West;
                }
                //PlayerSpriteRen.sprite = (moveInput.x > 0 ? right : left);
            }
			else if (Mathf.Abs(moveInput.x) <= Mathf.Abs(moveInput.y))
			{
				//Moving Up
				if (moveInput.y > 0)
				{
                    SpriteSetter(PlayerInstance.Instance.Facing == DirectionEnum.Direction.North, up);
                    PlayerInstance.Instance.Facing = DirectionEnum.Direction.North;
                } 
				//Moving Down
				else
				{
                    SpriteSetter(PlayerInstance.Instance.Facing == DirectionEnum.Direction.South, down);
                    PlayerInstance.Instance.Facing = DirectionEnum.Direction.South;
                }
				//PlayerSpriteRen.sprite = (moveInput.y > 0 ? up : down);
			}

		}

		private void SpriteSetter(bool sameFacing, Sprite[] spriteSet)
		{
            if (_spriteTimer == 0)
            {
                _spriteTimer = SpriteTimer;
				if (_currentSprite == spriteSet.Length - 1)
				{
					_currentSprite = 0;
				} 
				else
				{
                    _currentSprite++;
                }
            }

            if (sameFacing)
            {
                PlayerSpriteRen.sprite = spriteSet[_currentSprite];
				_spriteTimer--;
            }
            else
            {
                _currentSprite = 0;
                _spriteTimer = SpriteTimer;
				PlayerSpriteRen.sprite = spriteSet[_currentSprite];
            }
        }
	} 
}
