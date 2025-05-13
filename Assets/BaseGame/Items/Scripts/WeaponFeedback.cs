using System;
using UnityEngine;

namespace LCPS.SlipForge.Weapon
{
    [CreateAssetMenu(fileName = "NewWeaponFeedback", menuName = "BaseGame/Weapons/WeaponFeedback")]
    [Serializable]
    public class WeaponFeedback : ScriptableObject
    {
        [Header("Kickback Motion Feedback")]
        public AnimationCurve xFeedback;
        public AnimationCurve yFeedback;
        public AnimationCurve rotationFeedback;

        /// <summary>
        /// Gets the positional offset based on the provided time and weapon cycle duration.
        /// </summary>
        /// <param name="time">The current time in the feedback cycle.</param>
        /// <param name="cycleDuration">The duration of the weapon's firing cycle in seconds.</param>
        /// <returns>Vector2 representing X and Y offsets.</returns>
        public Vector3 EvaluatePosition(float time, float cycleDuration)
        {
            float normalizedTime = Mathf.Repeat(time, cycleDuration) / cycleDuration;
            float xOffset = xFeedback.Evaluate(normalizedTime);
            float yOffset = yFeedback.Evaluate(normalizedTime);
            return new Vector3(xOffset, yOffset, 0);
        }

        /// <summary>
        /// Gets the rotation offset based on the provided time and weapon cycle duration.
        /// </summary>
        /// <param name="time">The current time in the feedback cycle.</param>
        /// <param name="cycleDuration">The duration of the weapon's firing cycle in seconds.</param>
        /// <returns>Float representing the rotational offset in degrees.</returns>
        public float EvaluateRotation(float time, float cycleDuration)
        {
            float normalizedTime = Mathf.Repeat(time, cycleDuration) / cycleDuration;
            return rotationFeedback.Evaluate(normalizedTime);
        }
    }
}