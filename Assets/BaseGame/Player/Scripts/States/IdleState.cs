using UnityEngine;


namespace LCPS.SlipForge.Player
{

	public class IdleState : AbstractPlayerState
	{
		public Sprite idleUp;
        public Sprite idleRight;
        public Sprite idleDown;
        public Sprite idleLeft;

        protected override void Awake()
		{
			base.Awake();
			this.enabled = true;
		}

		protected override void OnEnable()
		{
			if (PlayerInstance.Instance.Facing == DirectionEnum.Direction.North)
			{
				PlayerSpriteRen.sprite = idleUp;
			}
			else if (PlayerInstance.Instance.Facing == DirectionEnum.Direction.East)
            {
                PlayerSpriteRen.sprite = idleRight;
            }
			else if (PlayerInstance.Instance.Facing == DirectionEnum.Direction.West)
            {
                PlayerSpriteRen.sprite = idleLeft;
            }
			else
			{
                PlayerSpriteRen.sprite = idleDown;
            }


        }
		protected override void OnDodge()
		{
			//don't allow dodge during idle
		}
		protected override void FixedUpdate()
		{
            base.FixedUpdate();
            if (_inputScheme.Player.Move.ReadValue<Vector2>().magnitude > 0)
			{
				GetComponent<WalkState>().enabled = true;
				this.enabled = false;
			}
			
		}
	}

}