using LCPS.SlipForge.Enum.Enemy;
using LCPS.SlipForge.Player;
using UnityEngine;

namespace LCPS.SlipForge.Enemy
{
    public class EvadeBehavior : PursuitBehavior
    {
        [SerializeField]
        private float Angle = 45f;

        private IWeaponRig _weaponRig;

        public override Vector3 CalculateMovement(Transform enemy, Transform player)
        {
            if (player == null)
            {
                return Vector3.zero;
            }

            if(PlayerInstance.Instance != null)
            {
                _weaponRig = PlayerInstance.Instance.WeaponRig;
            }

            if (_weaponRig == null)
            {
                return Vector3.zero;
            }

            // Get the player's aim direction
            var playerAimVector = _weaponRig.Forward;
            var directionToPlayer = (enemy.position - player.position).normalized;

            var angle = Vector3.SignedAngle(playerAimVector, directionToPlayer, Vector3.up);

            // Move perpendicular to the player's aim vector, magnitude depends on the ratio of 0 to Angle
            if (Mathf.Abs(angle) < Angle)
            {
                playerAimVector = angle < 0 ? -playerAimVector : playerAimVector;
                var evadeDirection = Vector3.Cross(Vector3.up, playerAimVector).normalized;

                // Adjust the evade vector based on the angle
                return evadeDirection * (Angle - Mathf.Abs(angle)) / Angle;
            }

            return Vector3.zero;
        }
    }
}
